using SocksCore.Primitives;
using SocksCore.Utils.Log;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocksCore
{
    public class TlvClientSourceFromListener : TlvClientSourceBase
    {

        private ICanLog logger;

        public TlvClientSourceFromListener(ICanLog log)
        {
            logger = log;
        }

        public void BeginAcceptClients(IPEndPoint listenTo)
        {
            logger.Trace($"Trying to start listen for new clients on: {listenTo.Address}  port:{listenTo.Port}");
            Task.Run(async () =>
            {

                var listener = new TcpListener(listenTo);
                listener.Start();
                while (true)
                {
                    try
                    {
                        var ex = await listener.AcceptTcpClientAsync().ConfigureAwait(false);
                        var s = new TcpClientEx();

                        s.AttachToSocket(ex.Client);
                        //s.Client.SetupSocketTimeouts(new SocketSettings { NetworkClientKeepAliveInterval = 1000, NetworkClientKeepAliveTimeout = 1000 });

                        var remoteEndPoint = s.Client.RemoteEndPoint as IPEndPoint;
                        if (remoteEndPoint != null)
                            logger.Trace($"Accepted a new client from {remoteEndPoint.Address}");
                        logger.Notice(
                            $"Client connection accepted from {remoteEndPoint?.Address}:{remoteEndPoint?.Port}");

                        OnNewSocksClientConnected(s);
                    }
                    catch (Exception e)
                    {
                        logger.Error(e.Message);
                    }
                }
            });
        }
    }
}
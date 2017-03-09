using SocksCore.Primitives;
using SocksCore.Utils.Log;
using System;
using System.Net;
using System.Net.Sockets;


namespace SocksCore.SocksHandlers.Socks4
{
    public abstract class Socks4ClientHandlerBase : SocksClientHandlerBase
    {
        private ICanLog logger;


        public override void HandleSocksRequest(ISocksClient clientToHandle)
        {
            logger.Trace($"Received new client request from {((IPEndPoint)(clientToHandle.Client.RemoteEndPoint)).Address}");
            var socks4Request = Socks4Request.From(clientToHandle);

            if (socks4Request.Header.RequestType == Socks4RequestType.TcpIpConnection)
            {
                logger.Notice($"Request command is TCP/IP connection to:{socks4Request.IpAddress} on:{socks4Request.Port}");
                var connectionTarget = new IPEndPoint(socks4Request.IpAddress, socks4Request.Port);

                try
                {
                    logger.Notice("Trying to connect");
                    clientToHandle.Connect(connectionTarget);
                }
                catch (SocketException e)
                {
                    logger.Error($"Connection to {socks4Request.IpAddress} on port {socks4Request.Port} thrown an exception:{Environment.NewLine}{e.Message}");
                    CloseConnectionAndSendError(clientToHandle, (uint)SocksCore.Socks4ErrorCodes.Error);
                    return;
                }

            }
            if (socks4Request.Header.RequestType == Socks4RequestType.PortBinding)
            {

                throw new NotImplementedException();
                // TODO: add port binding functionality
            }
            CloseConnectionAndSendError(clientToHandle, (uint)SocksCore.Socks4ErrorCodes.Error);

        }

        public Socks4ClientHandlerBase(ICanLog loggerToInitialize)
        {
            logger = loggerToInitialize;
        }
    }
}
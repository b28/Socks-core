using SocksCore.Primitives;
using SocksCore.Utils.Log;
using System;
using System.Net;
using System.Net.Sockets;


namespace SocksCore.SocksHandlers.Socks4
{
    public abstract class Socks4ClientHandler : TlvClietnHandlerBase
    {
        
        private ICanLog logger;

        protected override byte HeaderMarker => 0x04;

        public override bool CanHandleRequestByHeader(byte[] header)
        {
            return header[0] == HeaderMarker;
        }

        public override ISocksClient HandleClientRequest(ISocksClient clientToHandle)
        {
            logger.Trace($"Received new client request from {((IPEndPoint)(clientToHandle.Client.RemoteEndPoint)).Address}");
            var socks4Request = Socks4Request.From(clientToHandle);
            ISocks4Response result;

            if (socks4Request.Header.RequestType == Socks4RequestType.TcpIpConnection)
            {
                logger.Notice($"Request command is TCP/IP connection to:{socks4Request.IpAddress} on:{socks4Request.Port}");
                var connectionTarget = new IPEndPoint(socks4Request.IpAddress, socks4Request.Port);
                var targetConnection = new TcpClientEx();
                try
                {
                    logger.Notice($"Trying to connect to target {socks4Request.IpAddress}:{socks4Request.Port} through new socket");
                    targetConnection.Connect(connectionTarget);
                    //clientToHandle.Connect(connectionTarget);
                }
                catch (SocketException e)
                {
                    logger.Error($"Connection to {socks4Request.IpAddress} on port {socks4Request.Port} thrown an exception:{Environment.NewLine}{e.Message}");
                    //CloseConnectionAndSendError(clientToHandle, (uint)UniversalTlvCore.Socks4ErrorCodes.Error);
                    throw new ConnectionEstablisherException(); // catch in upper code
                }




                //var responseArray = response.ToByteArray();
                //clientStream.Write(responseArray, 0, responseArray.Length);

                //if (!fromConnection.Connection.Connected || !clientToTest.Connected) return;
                //var connectedStreams = new ConnectionPair(fromConnection, new ClientConnection(clientToTest),
                //    ConnectionIdentity.FromEndPoint((IPEndPoint)clientToTest.Client.RemoteEndPoint));
                //connectedStreams.OnSessionClosed += fromConnection.ConnectedStreamsOnSessionClosed;
                //fromConnection.CallMeOnConnect();
                //connectedStreams.ProcessProxyMapping();


            }
            if (socks4Request.Header.RequestType == Socks4RequestType.PortBinding)
            {

                throw new NotImplementedException();
                // TODO: add port binding functionality
            }
            //CloseConnectionAndSendError(clientToHandle, (uint)UniversalTlvCore.Socks4ErrorCodes.Error);

        }

        public Socks4ClientHandler(ICanLog loggerToInitialize)
        {
            logger = loggerToInitialize;
        }
    }
}
using SocksCore.Primitives;
using SocksCore.Utils.Log;
using System;
using System.Net;


namespace SocksCore.SocksHandlers.Socks4
{
    public class Socks4ClientHandler : SocksHandlerBase
    {

        private ICanLog logger;

        protected override byte HeaderMarker => 0x04;

        public override bool CanHandleRequestByHeader(byte[] header)
        {
            return header[0] == HeaderMarker;
        }

        public override void HandleClientRequest(ITlvClient clientToHandle)
        {
            logger.Trace($"Received new client request from {((IPEndPoint)(clientToHandle.Client.RemoteEndPoint)).Address}");
            var socks4Request = Socks4Request.From(clientToHandle);
            Socks4Response responseToClient;

            if (socks4Request.Header.RequestType == Socks4RequestType.TcpIpConnection)
            {
                //var ss = new byte[500];
                //var s = clientToHandle.Client.Receive(ss);
                logger.Notice($"Request command is TCP/IP connection to:{socks4Request.IpAddress} on:{socks4Request.Port}");
                var connectionTarget = new IPEndPoint(socks4Request.IpAddress, socks4Request.Port);
                var sourceSocksClient = new TcpClientEx();
                try
                {
                    logger.Notice($"Trying to connect to target {socks4Request.IpAddress}:{socks4Request.Port} through new socket");
                    sourceSocksClient.Connect(connectionTarget);
                    //clientToHandle.Connect(connectionTarget);
                }
                catch (Exception e)
                {
                    logger.Error($"Connection to {socks4Request.IpAddress} on port {socks4Request.Port} thrown an exception:{Environment.NewLine}{e.Message}");
                    //SendResponseToClient(clientToHandle, UniversalTlvCore.Socks4ErrorCodes.Error);
                    //clientToHandle.Close();
                    throw new ConnectionEstablisherException(); // catch in upper code
                }
                responseToClient = new Socks4Response(Socks4ErrorCodes.Success);
                clientToHandle.Send(responseToClient.GetBytes());

                var drainSocksClient = new TcpClientEx(clientToHandle.Client);
                // At this point we have an Drain and Source Socks "streams" which analog as a field transistor architecture.
                // and link them beetwen each other in LinkedPair instance
                var pair = new LinkedPairConnection(drainSocksClient, sourceSocksClient);
                //pair.SourceConnection.Disconnected+=pair.So;
                pair.JoinConnections();

                pair.LinkedPairClosed += PairOnLinkedPairClosed;
                LinkedConnections.Add(pair);

            }
            if (socks4Request.Header.RequestType == Socks4RequestType.PortBinding)
            {

                throw new NotImplementedException();
                // TODO: add port binding functionality
            }
            //CloseConnectionAndSendError(clientToHandle, (uint)UniversalTlvCore.Socks4ErrorCodes.Error);

        }

        private void PairOnLinkedPairClosed(object sender, ILinkedPairConnection linkedPairConnection)
        {
            LinkedConnections.Remove(linkedPairConnection);
            //ClientDisconnected

        }


        public Socks4ClientHandler(ICanLog loggerToInitialize)
        {
            logger = loggerToInitialize;
        }


    }
}
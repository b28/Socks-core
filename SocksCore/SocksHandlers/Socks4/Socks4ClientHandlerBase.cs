using SocksCore.Primitives;
using System;
using System.Net;
using System.Net.Sockets;



namespace SocksCore.SocksHandlers.Socks4
{
    public abstract partial class Socks4ClientHandlerBase : SocksClientHandlerBase
    {

        private ISocks4PacketFactory packetFactory;

        public override void HandleSocksRequest(ISocksClient clientToHandle)
        {

            var socks4Request = Socks4Request.From(clientToHandle);

            if (socks4Request.Header.RequestType == Socks4RequestType.TcpIpConnection)
            {
                var connectionTarget = new IPEndPoint(socks4Request.IpAddress, socks4Request.Port);

                try
                {
                    clientToHandle.Connect(connectionTarget);
                }
                catch (SocketException)
                {
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









        public interface ISocks4PacketFactory
        {
            Socks4RequestHeader GetRequestPacket(ISocksClient client);

        }
    }
}
namespace SocksCore.SocksHandlers.Socks4
{
    public abstract class Socks4ClientHandler : SocksClientHandler
    {

        private ISocks4PacketFactory packetFactory;

        public override void HandleSocksRequest(ISocksClient clientToHandle)
        {

            var socks4Request = Socks4Request.From(clientToHandle);




        }









        public interface ISocks4PacketFactory
        {
            Socks4RequestHeader GetRequestPacket(ISocksClient client);

        }
    }
}
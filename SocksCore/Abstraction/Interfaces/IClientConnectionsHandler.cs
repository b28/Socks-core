using SocksCore.Primitives;

namespace SocksCore
{
    public interface IClientConnectionsHandler
    {
        void SendResponseToClient(ITlvClient client, byte[] responsePacket);
    }
}
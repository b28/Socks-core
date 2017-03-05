namespace SocksCore
{
    public interface ISocksClientHandler : IClientConnectionsHandler
    {
        void HandleSocksRequest(ISocksClient clientToHandle);
    }
}
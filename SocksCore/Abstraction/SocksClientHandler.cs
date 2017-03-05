namespace SocksCore
{
    public abstract class SocksClientHandler : ClientConnectionHandler, ISocksClientHandler
    {

        public abstract void HandleSocksRequest(ISocksClient clientToHandle);
    }
}
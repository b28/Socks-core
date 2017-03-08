using SocksCore.Primitives;

namespace SocksCore
{
    public interface ISocksClientHandler : IClientConnectionsHandler, ISocketTimeoutsManager
    {
        ISocketHandlerSettings Settings { get; set; }
        void HandleSocksRequest(ISocksClient clientToHandle);

    }
}
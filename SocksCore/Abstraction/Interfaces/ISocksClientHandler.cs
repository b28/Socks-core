using SocksCore.Primitives;

namespace SocksCore
{
    public interface ISocksClientHandler : IClientConnectionsHandler, ISocketTimeoutsManager
    {
        ISocketHandlerSettings Settings { get; set; }
        ISocksClient HandleClientRequest(ISocksClient clientToHandle);
        bool CanHandleRequestByHeader(byte[] header);
    }
}
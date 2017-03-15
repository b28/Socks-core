using SocksCore.Primitives;

namespace SocksCore
{
    public interface ISocksClientHandler : IClientConnectionsHandler, ISocketTimeoutsManager
    {
        ISocketHandlerSettings Settings { get; set; }
        void HandleClientRequest(ITlvClient clientToHandle);
        bool CanHandleRequestByHeader(byte[] header);
    }
}
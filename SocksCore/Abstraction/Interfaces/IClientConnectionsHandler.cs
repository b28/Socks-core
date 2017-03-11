using SocksCore.Primitives;

namespace SocksCore
{
    public interface IClientConnectionsHandler
    {
        void CloseConnectionAndSendError(ISocksClient connectionToClose, uint errorCode);
    }
}
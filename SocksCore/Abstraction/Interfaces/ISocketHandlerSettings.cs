using SocksCore.Primitives;

namespace SocksCore
{

    public interface ISocketHandlerSettings
    {
        ISocketTimeouts ReceiveTimeout { get; set; }
        ISocketTimeouts SendTimeout { get; set; }
    }

}
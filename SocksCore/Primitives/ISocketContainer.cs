using System.Net.Sockets;

namespace SocksCore.Primitives
{
    public interface ISocketContainer
    {
        Socket Socket { get; }
    }
}
using SocksCore.Primitives;
using System.Net;

namespace SocksCore
{
    public interface ISocksConnectionEstablisher
    {
        /// <summary>
        /// Connects to requested endpoint (ip:port)
        /// throws <see cref="ConnectionEstablisherException"/> if timeout.
        /// </summary>
        /// <param name="connectTo">Connect to "endpoint"</param>
        /// <returns>Connected TcpClientEx</returns>
        TcpClientEx ConnectTo(IPEndPoint connectTo);
    }
}
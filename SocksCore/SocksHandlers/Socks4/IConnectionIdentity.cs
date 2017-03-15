using System.Net;

namespace SocksCore.SocksHandlers.Socks4
{
    public interface IConnectionIdentity
    {
        IPEndPoint DrainEndPoint { get; }
        IPEndPoint SourcEndPoint { get; }
    }

    public struct ConnectionIdentity : IConnectionIdentity
    {
        public ConnectionIdentity(IPEndPoint drain, IPEndPoint source)
        {
            DrainEndPoint = drain;
            SourcEndPoint = source;
        }
        public IPEndPoint DrainEndPoint { get; }
        public IPEndPoint SourcEndPoint { get; }
    }
}
using System.Net;

namespace SocksCore.SocksHandlers.Socks5
{
    public abstract class Socks5ClientHandler : SocksClientHandler
    {
        protected abstract IPAddress ResolveDomainName(string domainToResolve);
    }
}
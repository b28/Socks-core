using System.Net;

namespace SocksCore.SocksHandlers
{
    public abstract class Socks5ClientHandler : SocksClientHandler
    {
        protected abstract IPAddress ResolveDomainName(string domainToResolve);
    }
}
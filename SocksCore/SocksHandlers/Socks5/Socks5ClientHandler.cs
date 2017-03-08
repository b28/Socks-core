using System.Net;

namespace SocksCore.SocksHandlers.Socks5
{
    public abstract class Socks5ClientHandler : SocksClientHandlerBase
    {
        protected abstract IPAddress ResolveDomainName(string domainToResolve);
    }
}
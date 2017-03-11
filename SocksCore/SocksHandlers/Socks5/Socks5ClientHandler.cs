using System.Net;

namespace SocksCore.SocksHandlers.Socks5
{
    public abstract class Socks5ClientHandler : TlvClietnHandlerBase
    {
        protected abstract IPAddress ResolveDomainName(string domainToResolve);
        protected override byte HeaderMarker => 0x05;
    }


}
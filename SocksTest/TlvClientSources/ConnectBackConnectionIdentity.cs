using System.Net;
using SocksTest.Connectors;

namespace SocksTest.TlvClientSources
{
    public struct ConnectBackConnectionIdentity : IConnectBackConnectionIdentity
    {
        public const int IdentityBufferSize = 50;

        public override int GetHashCode()
        {
            unchecked
            {
                //var hashCode = RemoteEndPoint?.GetHashCode() ?? 0;
                var hashCode = InternalIp?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (WindowsVersion != null ? WindowsVersion.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (WindowsUserName != null ? WindowsUserName.GetHashCode() : 0);
                return hashCode;
            }
        }

        public IPAddress InternalIp { get; set; }
        public string WindowsVersion { get; set; }
        public string WindowsUserName { get; set; }

    }
}

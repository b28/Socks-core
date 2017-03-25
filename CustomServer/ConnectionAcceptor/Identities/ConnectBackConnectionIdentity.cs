using System.Net;

namespace CustomServer.ConnectionAcceptor.Identities
{
    public class ConnectBackConnectionIdentity : IConnectBackConnectionIdentity
    {
        public const int IdentityBufferSize = 50;

        public virtual bool Equals(ConnectBackConnectionIdentity other)
        {
            if (other == null)
                return false;
            return Equals(InternalIp, other.InternalIp) &&
                   string.Equals(WindowsVersion, other.WindowsVersion) &&
                   string.Equals(WindowsUserName, other.WindowsUserName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ConnectBackConnectionIdentity && Equals((ConnectBackConnectionIdentity)obj);
        }


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
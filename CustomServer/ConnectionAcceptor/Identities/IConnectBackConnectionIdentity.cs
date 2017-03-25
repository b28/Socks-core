using System.Net;

namespace CustomServer.ConnectionAcceptor.Identities
{
    public interface IConnectBackConnectionIdentity
    {
        IPAddress InternalIp { get; set; }
        string WindowsUserName { get; set; }
        string WindowsVersion { get; set; }
    }
}
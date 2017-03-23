using System.Net;

namespace SocksTest.Connectors
{
    public interface IConnectBackConnectionIdentity
    {
        IPAddress InternalIp { get; set; }
        string WindowsUserName { get; set; }
        string WindowsVersion { get; set; }
    }
}
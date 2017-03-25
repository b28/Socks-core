using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocksTest.Connectors
{
    public interface IIdentityFactory
    {
        //IConnectBackConnectionIdentity ContextIdentity(ConnectBackConnectionIdentity identity, ConnectBackContext backContext);
        Task<IConnectBackConnectionIdentity> ConnectionIdentityFrom(TcpClient socket);
    }

    public class BackConnectorIdentityFactory : IIdentityFactory
    {
        public Task<IConnectBackConnectionIdentity> ConnectionIdentityFrom(TcpClient socket)
        {
            throw new System.NotImplementedException();
        }
    }
}
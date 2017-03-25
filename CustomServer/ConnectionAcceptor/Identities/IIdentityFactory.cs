using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CustomServer.Connections.Primitives;

namespace CustomServer.ConnectionAcceptor.Identities
{
    interface IIdentityFactory
    {
        ConnectBackContextIdentity ContextIdentity(ConnectBackConnectionIdentity identity, ConnectBackContext backContext);
        Task<ConnectBackConnectionIdentity> ConnectionIdentity(TcpClientEx socket);
    }
}

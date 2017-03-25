using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomServer.ConnectionAcceptor.Identities;

namespace CustomServer.ConnectionAcceptor.Server
{
    public interface IContextFactory
    {
        ConnectBackContext FromConnectionIdentity(ConnectBackConnectionIdentity connectionIdentity);
    }
}

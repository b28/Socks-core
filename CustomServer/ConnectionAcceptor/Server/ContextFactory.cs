using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomServer.ConnectionAcceptor.Identities;

namespace CustomServer.ConnectionAcceptor.Server
{
    internal class ContextFactory : IContextFactory
    {
        public ConnectBackContext FromConnectionIdentity(ConnectBackConnectionIdentity connectionIdentity)
        {
            var context = new ConnectBackContext(connectionIdentity);
            return context;
        }
    }
}

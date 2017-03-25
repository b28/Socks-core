using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomServer.Connections.Primitives;

namespace CustomServer.Connections.Clients
{
    public class ExternalConnection : TcpClientEx
    {
        public ExternalConnection(bool setKeepAlive) : base(setKeepAlive)
        {
        }




    }
}

using System.Net;

namespace CustomServer.ConnectionAcceptor.Server
{
    public class ServerContext
    {
        public IPEndPoint InternalEndPoint;
        public IPEndPoint ExternalEndpoint;

        public ServerContext(IPEndPoint externalEndPoint, IPEndPoint internalEndPoint)
        {
            InternalEndPoint = internalEndPoint;
            ExternalEndpoint = externalEndPoint;
        }
    }
}
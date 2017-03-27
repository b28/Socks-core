using SocksCore.SocksHandlers.Socks4;
using SocksCore.Utils;
using System;
using System.Linq;
using System.Net;

namespace SocksCore.SocksHandlers
{
    public abstract class SocksHandlerBase : TlvClientHandlerBase
    {
        protected readonly ConcurrentList<ILinkedPairConnection> LinkedConnections = new ConcurrentList<ILinkedPairConnection>();

        public void RegisterLinkedPair(ILinkedPairConnection pairToRegister)
        {
            LinkedConnections.Add(pairToRegister);

        }

        public void RemoveLinkedPair(ILinkedPairConnection pairToRemove)
        {
            LinkedConnections.Remove(pairToRemove);
        }

        public event EventHandler<IConnectionIdentity> ClientDisconnected;
        public int RegisterLinkedConnection(ILinkedPairConnection connectionToRegister)
        {
            if (connectionToRegister != null)
                LinkedConnections.Add(connectionToRegister);
            return LinkedConnections.Count();
        }

        public void RemoveLinkedConnectionByDestinationEndPoint(IPEndPoint markerEndPoint)
        {
            foreach (var linkedPairConnection in LinkedConnections)
            {
                if (Equals(linkedPairConnection.DrainConnection.ConnectedToEndPoint, markerEndPoint))
                    LinkedConnections.Remove(linkedPairConnection);
            }
        }

    }
}
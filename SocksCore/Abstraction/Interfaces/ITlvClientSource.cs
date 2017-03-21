using SocksCore.Primitives;
using System;
using System.Net;

namespace SocksCore
{
    public interface ITlvClientSource
    {
        event EventHandler<ITlvClient> NewTlvClientConnected;
    }

    public abstract class TlvClientSourceBase : ITlvClientSource
    {

        public event EventHandler<ITlvClient> NewTlvClientConnected;
        protected virtual void OnNewTlvClientConnected(ITlvClient e)
        {
            NewTlvClientConnected?.Invoke(this, e);
        }
        public abstract void GetClients(IPEndPoint getClientsFrom);

    }

}
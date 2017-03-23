using SocksCore.Primitives;
using System;

namespace SocksCore
{
    public interface ITlvClientSource
    {
        event EventHandler<ITlvClient> NewTlvClientConnected;
    }

    public abstract class TlvClientSourceBase : ITlvClientSource
    {
        public abstract void StartConnections();
        public event EventHandler<ITlvClient> NewTlvClientConnected;
        protected virtual void OnNewTlvClientConnected(ITlvClient e)
        {
            NewTlvClientConnected?.Invoke(this, e);
        }

    }

}
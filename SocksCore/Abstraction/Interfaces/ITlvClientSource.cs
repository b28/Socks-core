using SocksCore.Primitives;
using System;

namespace SocksCore
{
    public interface ITlvClientSource
    {
        event EventHandler<ITlvClient> NewTlvClientConnected;
    }

    public class TlvClientSourceBase : ITlvClientSource
    {
        public event EventHandler<ITlvClient> NewTlvClientConnected;
        protected virtual void OnNewSocksClientConnected(ITlvClient e)
        {
            NewTlvClientConnected?.Invoke(this, e);
        }
    }


}
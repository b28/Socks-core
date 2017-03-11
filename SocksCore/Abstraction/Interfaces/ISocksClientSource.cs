using SocksCore.Primitives;
using System;

namespace SocksCore
{
    public interface ISocksClientSource
    {
        event EventHandler<ISocksClient> NewSocksClientConnected;
    }

    public class SocksClientSourceBase : ISocksClientSource
    {
        public event EventHandler<ISocksClient> NewSocksClientConnected;
        protected virtual void OnNewSocksClientConnected(ISocksClient e)
        {
            NewSocksClientConnected?.Invoke(this, e);
        }
    }


}
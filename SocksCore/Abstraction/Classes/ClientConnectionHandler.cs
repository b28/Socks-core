using SocksCore.Primitives;
using System;

namespace SocksCore
{
    public abstract class ClientConnectionHandler : IClientConnectionsHandler
    {
        public virtual void CloseConnectionAndSendError(ISocksClient connectionToClose, uint errorCode)
        {
            var errorArray = BitConverter.GetBytes(errorCode);
            try
            {
                connectionToClose.Send(errorArray);
            }
            finally
            {
                connectionToClose.Close();
            }
        }


    }
}
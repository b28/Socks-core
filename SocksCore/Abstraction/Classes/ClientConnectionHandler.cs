using SocksCore.Primitives;
using System;

namespace SocksCore
{
    public abstract class ClientConnectionHandler : IClientConnectionsHandler
    {
        public virtual void SendResponseToClient(ITlvClient client, byte[] responsePacket)
        {
            var errorArray = (responsePacket);
            try
            {
                client.Send(errorArray);
            }
            finally
            {
                client.Close();
            }
        }


    }
}
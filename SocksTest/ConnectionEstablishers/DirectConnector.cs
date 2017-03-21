using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SocksCore;

namespace SocksTest.ConnectionEstablishers
{
    public class DirectConnector : TlvClientSourceBase , IConnectionEstablisher
    {

        public event EventHandler<string> DebugMessage;


        public TcpClient Connect(IPEndPoint ipEndPoint, Socket socketToConnect)
        {
            var targetConnection = new TcpClient(AddressFamily.InterNetwork) { Client = socketToConnect };
            Task.Run( async () =>
            {
                await targetConnection.ConnectAsync(ipEndPoint.Address,  ipEndPoint.Port).ConfigureAwait(false);
                return targetConnection;
            }
            ).ConfigureAwait(false);
            return targetConnection;
        }

        public override void GetClients(IPEndPoint getClientsFrom)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Net;
using System.Net.Sockets;
using SocksTest.ConnectionEstablisher;

namespace SocksTest.ConnectionEstablishers
{
    public class DirectConnectionEstablisher : IConnectionEstablisher
    {
        public TcpClient Connect(IPEndPoint ipEndPoint)
        {
            try
            {
                var client = new TcpClient();
                client.Connect(ipEndPoint);
                DebugMessage?.Invoke(this, $"Succesfully Connected to {ipEndPoint.Address}:{ipEndPoint.Port}");
                return client;

            }
            catch (Exception)
            {
                DebugMessage?.Invoke(this, $"Can't connect to {ipEndPoint.Address}:{ipEndPoint.Port}. Aborting");
                return null;
            }
        }

        public event EventHandler<string> DebugMessage;
    }
}


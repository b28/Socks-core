using System;
using System.Net.Sockets;

namespace SocksTest.Connectors.Connections
{
    public interface IBackConnection : IConnection
    {
        //void BeginPoll();
        event EventHandler HasDataToRead;
    }

    public interface IConnection
    {
        TcpClient Connection { get; set; }

        event EventHandler<BackConnection> Disconnected;
        event EventHandler<BackConnection> Connected;
    }
}
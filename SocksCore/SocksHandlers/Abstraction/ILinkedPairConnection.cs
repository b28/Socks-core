using SocksCore.Primitives;
using SocksCore.SocksHandlers.Socks4;
using System;
using System.Net;
using System.Net.Sockets;

namespace SocksCore.SocksHandlers
{
    public interface ILinkedPairConnection
    {
        TcpClientEx DrainConnection { get; }
        TcpClientEx SourceConnection { get; }
        IConnectionIdentity Identity { get; }
        event EventHandler<ITlvClient> SourceConnectionClosed;
        event EventHandler<ILinkedPairConnection> LinkedPairClosed;
    }

    public class LinkedPairConnection : ILinkedPairConnection
    {
        private bool disposed;
        private const int BufferSize = 32 * 1024;
        public IConnectionIdentity Identity { get; }
        public LinkedPairConnection(TcpClientEx drainConnection, TcpClientEx sourceConnection)
        {
            DrainConnection = drainConnection;
            SourceConnection = sourceConnection;
            Identity = new ConnectionIdentity(DrainConnection.Client.RemoteEndPoint as IPEndPoint, SourceConnection.Client.RemoteEndPoint as IPEndPoint);
        }
        public TcpClientEx DrainConnection { get; }
        public TcpClientEx SourceConnection { get; }
        public event EventHandler<ITlvClient> SourceConnectionClosed;
        public event EventHandler<ILinkedPairConnection> LinkedPairClosed;

        public void JoinConnections()
        {
            //JoinSessions(DrainConnection, SourceConnection);
            //JoinSessions(SourceConnection, DrainConnection);

            SourceConnection.DataReceived += SourceConnectionOnDataReceived;
            DrainConnection.DataReceived += DrainConnectionOnDataReceived;
            SourceConnection.Disconnected += SourceConnectionOnDisconnected;
            
            DrainConnection.BeginReceive();
            SourceConnection.BeginReceive();
        }

        private void SourceConnectionOnDisconnected(object sender, EventArgs eventArgs)
        {
            CloseSession();
        }

        private void DrainConnectionOnDataReceived(object sender, PacketData packetData)
        {
            SourceConnection.Client.Send(packetData.Buffer, 0, packetData.BytesCount, SocketFlags.None);
        }

        private void SourceConnectionOnDataReceived(object sender, PacketData packetData)
        {
            DrainConnection.Client.Send(packetData.Buffer, 0, packetData.BytesCount, SocketFlags.None);
        }

        private void CloseSession()
        {
            if (disposed) return;
            disposed = true;
            SourceConnection.Client?.Close();
            DrainConnection.Client?.Close();

            SourceConnection.DataReceived -= SourceConnectionOnDataReceived;
            SourceConnection.Disconnected -= SourceConnectionOnDisconnected;

            DrainConnection.DataReceived -= DrainConnectionOnDataReceived;

            DoOnSessionClosed();
        }

        private void DoOnSessionClosed()
        {
            LinkedPairClosed?.Invoke(this, this);
        }


    }
}
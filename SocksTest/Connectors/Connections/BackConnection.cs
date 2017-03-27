using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocksTest.Connectors.Connections
{
    public class BackConnection : IBackConnection
    {
        private const int BufferSize = 1024;
        public event EventHandler HasDataToRead;

        public static BackConnection From(TcpClient connectionSource)
        {
            return new BackConnection(connectionSource);
        }

        private BackConnection(TcpClient attachTo)
        {
            Connection = attachTo;
        }
        public async Task BeginPollAsync()
        {

            await Task.Run(() =>

                {
                    try
                    {
                        var buffer = new byte[BufferSize];
                        var polledBytesCount = Connection.Client.Receive(buffer, SocketFlags.Peek);
                        if (polledBytesCount <= 0)
                            OnDisconnected(this);
                        OnHasDataToRead();
                    }
                    catch (Exception e)
                    {
                        OnDisconnected(this);
                    }

                }
            ).ConfigureAwait(false);
        }

        public TcpClient Connection { get; set; }
        public event EventHandler<BackConnection> Disconnected;
        public event EventHandler<BackConnection> Connected;

        protected virtual void OnDisconnected(BackConnection e)
        {
            Disconnected?.Invoke(this, e);
        }

        protected virtual void OnHasDataToRead()
        {
            HasDataToRead?.Invoke(this, EventArgs.Empty);
        }
    }
}
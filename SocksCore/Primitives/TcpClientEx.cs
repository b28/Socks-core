using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocksCore.Primitives
{


    public struct DataStruct
    {
        public byte[] Buffer;
        public int BytesCount;

    }

    public class TcpClientEx : TcpClient, ISocksClient
    {
        private const int BufferSize = 32 * 1024; // 32KB


        private byte[] ReadBuffer = new byte[BufferSize];


        private const uint KeepAliveTimeout = 10000;
        private const uint KeepAliveInterval = 10000;

        public event EventHandler Disconnected;
        public event EventHandler<DataStruct> DataReceived;

        public TcpClientEx()
        {

        }
        public TcpClientEx(Socket attachTo)
        {
            AttachToSocket(attachTo);
        }
        public void AttachToSocket(Socket socketToAttach)
        {
            if (socketToAttach == null)
                throw new ArgumentNullException($"{nameof(socketToAttach)} is null");
            Client = socketToAttach;
            Client.SetupSocketTimeouts(new SocketSettings
            {
                NetworkClientKeepAliveInterval = KeepAliveInterval,
                NetworkClientKeepAliveTimeout = KeepAliveTimeout
            });
        }

        public async void BeginReceive()
        {

            while (Client != null && Client.Connected)
            {
                await BeginReceive(ReadBuffer);
            }

        }

        protected async Task BeginReceive(byte[] readTo)
        {
            try
            {

                var stream = GetStream();

                var readedBytesCount = await stream.ReadAsync(readTo, 0, readTo.Length).ConfigureAwait(false);
                if (readedBytesCount >= 1)
                {
                    DataReceived?.Invoke(this, new DataStruct
                    {
                        Buffer = readTo,
                        BytesCount = readedBytesCount
                    });
                }
                else
                {
                    DoOnDisconnected();
                }
            }
            catch (Exception)
            {
                Client.Close();
                DoOnDisconnected();
            }
        }


        protected virtual void DoOnDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        public byte[] PeekBytes(int bytesCount)
        {
            throw new NotImplementedException();
        }

        public byte[] Receive(int bytesCount)
        {
            throw new NotImplementedException();
        }

        public void Send(byte[] errorArray)
        {
            throw new NotImplementedException();
        }
    }
}
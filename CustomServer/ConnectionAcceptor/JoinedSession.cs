using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using CustomServer.Connections.Primitives;

namespace CustomServer.ConnectionAcceptor
{
    public sealed class JoinedSession
    {

        public int ReceiveBufferSize = 1024 * 1024;

        public event EventHandler<JoinedSession> OnSessionClosed;



        private const int BufferSize = 1024 * 32; // 10K

        public readonly TcpClientEx internalClient;
        public readonly TcpClientEx connectBackClient;
        //private bool disposed;



        public JoinedSession(TcpClientEx internalClient, TcpClientEx connectBackClient)
        {
            this.internalClient = internalClient;
            this.connectBackClient = connectBackClient;

            this.internalClient.Client.SetupSocketTimeouts(new SocketSettings());
        }

        public void ProcessProxyMapping()
        {
            Task.Factory.StartNew(JoinSessions);
            //JoinSessions(internalClient, connectBackClient);
        }

        private void JoinSessions(/*TcpClientEx fromClient, TcpClientEx toClient*/)
        {
            try
            {
                /*
                var fromBackClientBuffer = new byte[BufferSize];

                while (fromClient.Connected && toClient.Connected)
                {
                    var fromStream = fromClient.GetStream();
                    var readedBytesCount =
                        await
                            fromStream.ReadAsync(fromBackClientBuffer, 0, fromBackClientBuffer.Length)
                                .ConfigureAwait(false);

                    if (readedBytesCount < 1)
                    {
                        throw new SocketException();
                    }

                    var toStream = toClient.GetStream();

                    await toStream.WriteAsync(fromBackClientBuffer, 0, readedBytesCount).ConfigureAwait(false);
                }
                */



                connectBackClient.DataReceived += ConnectBackClientOnDataReceived; //(sender, bytes) => { fromClient.Client.Send(bytes); };
                internalClient.DataReceived += InternalClientOnDataReceived; //(sender, bytes) => { toClient.Client.Send(bytes); };

                internalClient.BeginReceive(); // calls here, coz event OnDataReceive stil not affected

                internalClient.Disconnected +=    FromClientOnDisconnected;
                connectBackClient.Disconnected += FromClientOnDisconnected;
            }
            catch (Exception)
            {
                CloseSession();
            }
            //finally
            //{
            //    OnSessionClosed?.Invoke(this, this);
            //}
        }

        private void ConnectBackClientOnDataReceived(object sender, DataStruct dataStructure)
        {
            internalClient.Client.Send(dataStructure.Buffer,0,dataStructure.BytesCount,SocketFlags.None);
        }

        private void InternalClientOnDataReceived(object sender, DataStruct dataStructure)
        {
            connectBackClient.Client.Send(dataStructure.Buffer, 0, dataStructure.BytesCount, SocketFlags.None);
        }


        private void FromClientOnDisconnected(object sender, EventArgs e)
        {
            CloseSession();
        }

        private void CloseSession()
        {

            if (connectBackClient != null) connectBackClient.DataReceived   -= ConnectBackClientOnDataReceived;
            if (internalClient != null) internalClient.DataReceived         -= InternalClientOnDataReceived;

            internalClient?.Close();
            connectBackClient?.Close();

            DoOnSessionClosed();
        }

        private void DoOnSessionClosed()
        {
            
            OnSessionClosed?.Invoke(this, this);
        }
    }

}
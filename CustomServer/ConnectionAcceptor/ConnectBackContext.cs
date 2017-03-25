using CustomServer.ConnectionAcceptor.Identities;
using CustomServer.Connections.Primitives;
using CustomServer.Utils;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CustomServer.ConnectionAcceptor
{
    public sealed class ConnectBackContext //per single MACHINE!! but MANY connections
    {
        CancellationTokenSource needToListen;
        private bool hasAnyConnection => availableExternalConnections.Any() || joinedSessions.Any();

        public readonly ConnectBackConnectionIdentity ConnectionIdentity;
        public TcpListener listener;

        private readonly ConcurrentList<TcpClientEx> availableExternalConnections = new ConcurrentList<TcpClientEx>();
        private readonly ConcurrentList<JoinedSession> joinedSessions = new ConcurrentList<JoinedSession>();

        public event EventHandler<ConnectBackContext> ClientDisconnected;

        public int PortToConnect => ((IPEndPoint)listener.LocalEndpoint).Port;

        //IpEndPoint)listener.)


        public ConnectBackContext(ConnectBackConnectionIdentity id)
        {

            ConnectionIdentity = id;
            listener = new TcpListener(new IPEndPoint(IPAddress.Any, 0));
        }


        public void Start()
        {

            listener.Start();
            needToListen = new CancellationTokenSource();
            //Debug.WriteLine($"New Connect Back Client remote IP:{ConnectionIdentity.RemoteEndPoint}/Internal IP:{ConnectionIdentity.InternalIp}/ Port is:" + ((IPEndPoint)(listener.LocalEndpoint)).Port);
            AcceptInternalClientLoop(needToListen.Token);
        }

        public void StopInternalListener()
        {
            needToListen.Cancel();
        }


        private async void AcceptInternalClientLoop(CancellationToken token)
        {

            while (!token.IsCancellationRequested)
            {
                var realTcpClient = await listener.AcceptTcpClientAsync();
                var realClient = new TcpClientEx(true);
                realClient.AttachSocket(realTcpClient.Client);
                //realClient.Client.SetupSocketTimeouts(new SocketSettings { NetworkClientKeepAliveTimeout = 1000, NetworkClientKeepAliveInterval = 1000 });

                var connectBackConnection = availableExternalConnections.FirstOrDefault();
                if (connectBackConnection != null)
                {
                    availableExternalConnections.Remove(connectBackConnection);
                    connectBackConnection.Disconnected -= TcpClientOnDisconnected;
                    var joinedSession = new JoinedSession(realClient, connectBackConnection);
                    joinedSessions.Add(joinedSession);
                    joinedSession.OnSessionClosed += JoinedSessionOnOnSessionClosed;
                    joinedSession.ProcessProxyMapping();
                }
                else
                {
                    realClient.Client.Send(Encoding.ASCII.GetBytes("No external connection, closing."));
                    realClient.Close();
                }
                //needToListen.ThrowIfCancellationRequested();
            }

            // ReSharper disable once FunctionNeverReturns
        }

        private void JoinedSessionOnOnSessionClosed(object sender, JoinedSession session)
        {
            session.OnSessionClosed -= JoinedSessionOnOnSessionClosed;

            joinedSessions.Remove(session);

            if (hasAnyConnection) return;
            StopInternalListener();
            OnClientDisconnected(this);
        }



        public void RegisterCallbackClient(TcpClientEx tcpClient)
        {
            availableExternalConnections.Add(tcpClient);
            tcpClient.Disconnected += TcpClientOnDisconnected;
            tcpClient.BeginReceive();
        }

        private void TcpClientOnDisconnected(object sender, EventArgs eventArgs)
        {
            var disconnectedConnection = sender as TcpClientEx;

            needToListen.Cancel();
            

            availableExternalConnections.Remove(disconnectedConnection);

            if (!availableExternalConnections.Any()) // if no more free connections available
                OnClientDisconnected(this);
        }

        private void OnClientDisconnected(ConnectBackContext e)
        {
            ClientDisconnected?.Invoke(this, e);
        }
    }
}
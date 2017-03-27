using CustomServer.ConnectionAcceptor.Identities;
using CustomServer.ConnectionAcceptor.Server;
using CustomServer.Connections.Primitives;
using CustomServer.Utils;
using log4net;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CustomServer.ConnectionAcceptor
{



    public sealed class AsynchronousSocketListener
    {
        //public ConcurrentList<IdenticalConections> Connections = new ConcurrentList<IdenticalConections>();


        #region Fields

        private ConcurrentList<ConnectBackContext> ConnectBackContexts = new ConcurrentList<ConnectBackContext>();
        private TcpListener listener;

        #endregion

        ILog logger;
        IIdentityFactory IdentityFactory = new IdentityFactory();
        IContextFactory ContextFactory = new ContextFactory();

        /// <summary>
        /// Raise when new back client connected, and notify.
        /// </summary>
        public event EventHandler<ConnectBackContext> RemoteClientConnected;
        /// <summary>
        /// Raise when back client disconnected, and notify.
        /// </summary>
        public event EventHandler<ConnectBackContext> RemoteClientDisconnected;

        public event EventHandler<string> DebugAction;

        public AsynchronousSocketListener(ILog log)
        {
            logger = log;
        }


        private void Message(string msg)
        {
            DebugAction?.Invoke(this, msg);
        }


        private void AcceptExternalConnectionsLoop()
        {


            Task.Run(async () =>
             {
                 while (true)
                 {
                     try
                     {
                         var ex = await listener.AcceptTcpClientAsync().ConfigureAwait(false);
                         var s = new TcpClientEx(true);


                         s.AttachSocket(ex.Client);
                         //s.Client.SetupSocketTimeouts(new SocketSettings { NetworkClientKeepAliveInterval = 1000, NetworkClientKeepAliveTimeout = 1000 });

                         var remoteEndPoint = s.Client.RemoteEndPoint as IPEndPoint;
                         if (remoteEndPoint != null) logger.Info($"Accepted a new client from {remoteEndPoint.Address}");
                         Message($"Client connection accepted from {remoteEndPoint?.Address}:{remoteEndPoint?.Port}");

                         StartSession(s);
                     }
                     catch (Exception e)
                     {
                         Message(e.Message);
                     }
                 }

             });

        }


        /// <summary>
        /// Identifying backclient.
        /// </summary>
        /// <param name="tcpClient"></param>
        private async void StartSession(TcpClientEx tcpClient)
        {
            logger.Info("Start new session");
            // get client identify (Internal/external IP)
            var identity = await IdentityFactory.ConnectionIdentity(tcpClient);

            if (identity != null)
            {
                ConnectBackContext context;

                lock (this)
                {
                    context = GetContextByIdentity(identity) ?? RegisterConnectBackContext(identity);
                }

                context.RegisterCallbackClient(tcpClient);
                logger.Info($"New client identity received: {identity}");
                context.ClientDisconnected += ContextOnClientDisconnected;

            }
            else
            {
                tcpClient.Client.Close();
                logger.Error("Can't identify remote client, connected from");
                Message("Can't identify remote client.");
            }
        }

        private void ContextOnClientDisconnected(object sender, ConnectBackContext disconnectedContext)
        {
            logger.Info($"Client disconnected {disconnectedContext.PortToConnect}");
            var context = sender as ConnectBackContext;
            if (context == null) return;

            context.ClientDisconnected -= ContextOnClientDisconnected;
            OnRemoteClientDisconnected(disconnectedContext);
        }



        private ConnectBackContext RegisterConnectBackContext(ConnectBackConnectionIdentity id)
        {

            var newContext = ContextFactory.FromConnectionIdentity(id);

            //ConnectBackContexts.Add(result);
            ConnectBackContexts.Add(newContext);

            newContext.Start();

            OnRemoteClientConnected(newContext);
            logger.Info($"Registering new ConnectBack context at internal port {newContext.PortToConnect}");
            return newContext;
        }

        private ConnectBackContext GetContextByIdentity(ConnectBackConnectionIdentity id)
        {
            return ConnectBackContexts.FirstOrDefault(t => t.ConnectionIdentity.Equals(id));
        }

        public void StartListening(IPEndPoint listenTo)
        {
            logger.Info($"Trying to start listen for new clients on: {listenTo.Address}  port:{listenTo.Port}");
            try
            {
                listener = new TcpListener(listenTo);
                listener.Start();
            }
            catch (SocketException s)
            {
                Message(s.Message);
            }
            try
            {
                AcceptExternalConnectionsLoop();
            }
            catch (Exception e)
            {
                Message($"Client accept error: {e}");
            }

        }

        public static class ArrayExtension
        {
            public static string GetString(byte[] bytes)
            {
                var line = Encoding.ASCII.GetString(bytes, 0, bytes.Length);

                return line;
            }
        }

        public class SessionException : Exception
        {
            public SessionException()
            {

            }

            public SessionException(string message)
                : base(message)
            {
            }

            public SessionException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }


        private void OnRemoteClientConnected(ConnectBackContext e)
        {
            RemoteClientConnected?.Invoke(this, e);
        }


        private void OnRemoteClientDisconnected(ConnectBackContext e)
        {
            ConnectBackContexts.Remove(e);
            RemoteClientDisconnected?.Invoke(this, e);
        }


    }


}
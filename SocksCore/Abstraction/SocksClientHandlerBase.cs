using SocksCore.Primitives;

namespace SocksCore
{
    public abstract class SocksClientHandlerBase : ClientConnectionHandler, ISocksClientHandler
    {
        public abstract void HandleSocksRequest(ISocksClient clientToHandle);
        public ISocketTimeouts GetTimeouts(ISocketContainer socket)
        {
            var oldTimeouts = new SocketTimeouts { ReceiveTimeout = socket.Socket.ReceiveTimeout, SendTimeout = socket.Socket.SendTimeout };
            return oldTimeouts;
        }

        public ISocketTimeouts SetTimeouts(ISocketContainer container, ISocketTimeouts timeouts)
        {
            var oldTimeouts = GetTimeouts(socket: container);
            container.Socket.ReceiveTimeout = timeouts.ReceiveTimeout;
            container.Socket.SendTimeout = timeouts.SendTimeout;
            return oldTimeouts;
        }

        public ISocketHandlerSettings Settings { get; set; }
    }
}
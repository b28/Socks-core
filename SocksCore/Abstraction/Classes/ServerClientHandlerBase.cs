using SocksCore.Primitives;

namespace SocksCore
{
    public abstract class TlvClientHandlerBase : ClientConnectionHandler
    {
        protected abstract byte HeaderMarker { get; }
        public ISocketHandlerSettings Settings { get; set; }
        public abstract void HandleClientRequest(ITlvClient clientToHandle);
        
        public abstract bool CanHandleRequestByHeader(byte[] header);
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

    }
}
using SocksCore.Primitives;
using SocksCore.Utils.Log;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocksCore
{
    public sealed class UniversalTlvCore : IClientConnectionsHandler
    {
        private const int TlvHeaderSize = 1;
        private IList<TlvClientHandlerBase> registeredClientHandlers = new List<TlvClientHandlerBase>();

        public int ActiveConnections { get; private set; }
        //private const byte DefaultSocksError = (byte)Socks4ErrorCodes.Error;

        public void AcceptClientConnection(ITlvClient client /*Socket client*/)
        {
            var tlvPacketType = TlvTypeFromHeader(client);

            var currentHandler =
                registeredClientHandlers.First(
                    registeredClientHandler => registeredClientHandler.CanHandleRequestByHeader(BitConverter.GetBytes(tlvPacketType))
                    );

            if (currentHandler == null)
                throw new TlvCoreException("No registered handlers for this Packet type");

            currentHandler.HandleClientRequest(client);
        }

        private static byte TlvTypeFromHeader(IBytePeeker peeker)
        {
            var readedVersion = peeker.PeekBytes(TlvHeaderSize);
            return readedVersion.First();
        }

        private ICanLog logger;
        public UniversalTlvCore(ICanLog log, params TlvClientHandlerBase[] handlersToAdd)
        {
            logger = log;
            AddHandlers(handlersToAdd);
        }

        public int AddHandlers(params TlvClientHandlerBase[] handlersToAdd)
        {
            foreach (var clientConnectionsHandler in handlersToAdd)
                if (clientConnectionsHandler != null)
                    registeredClientHandlers.Add(clientConnectionsHandler);
            return registeredClientHandlers.Count;
        }

        public event EventHandler<string> ConnectionEstablished;
        public event EventHandler<string> Disconnected;


        public void SendResponseToClient(ITlvClient client, byte[] responsePacket)
        {
            client.Close();
        }

        #region Events invocators

        private void OnClientConnected(string e)
        {
            ActiveConnections++;
            ConnectionEstablished?.Invoke(this, e);
        }

        private void OnClientDisconnected(string e)
        {
            Disconnected?.Invoke(this, e);
        }



        #endregion

    }
}

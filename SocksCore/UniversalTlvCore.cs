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
        private IList<TlvClietnHandlerBase> registeredClientHandlers = new List<TlvClietnHandlerBase>();

        private const byte DefaultSocksError = (byte)Socks4ErrorCodes.Error;

        public void AcceptClientConnection(ISocksClient client /*Socket client*/)
        {
            var tlvPacketType = TlvTypeFromHeader(client);

            var currentHandler =
                registeredClientHandlers.FirstOrDefault(
                    registeredClientHandler => registeredClientHandler.CanHandleRequestByHeader(BitConverter.GetBytes(tlvPacketType))
                    );

            if (currentHandler == null)
                CloseConnectionAndSendError(client, DefaultSocksError);
            currentHandler?.HandleClientRequest(client);
        }

        private static byte TlvTypeFromHeader(IBytePeeker peeker)
        {
            var readedVersion = peeker.PeekBytes(TlvHeaderSize);
            return readedVersion.First();
        }

        private ICanLog logger;
        public UniversalTlvCore(ICanLog log, params TlvClietnHandlerBase[] handlersToAdd)
        {
            logger = log;
            AddHandlers(handlersToAdd);
        }

        public int AddHandlers(params TlvClietnHandlerBase[] handlersToAdd)
        {
            foreach (var clientConnectionsHandler in handlersToAdd)
                if (clientConnectionsHandler != null)
                    registeredClientHandlers.Add(clientConnectionsHandler);
            return registeredClientHandlers.Count;
        }

        public event EventHandler<string> ConnectionEstablished;
        public event EventHandler<string> Disconnected;


        public void CloseConnectionAndSendError(ISocksClient connectionToClose, uint errorCode)
        {
            connectionToClose.Close();
            //            throw new NotImplementedException();
        }

        #region Events invocators

        private void OnClientConnected(string e)
        {
            ConnectionEstablished?.Invoke(this, e);
        }

        private void OnClientRequestReceived(string e)
        {
            Disconnected?.Invoke(this, e);
        }



        #endregion

    }
}

using System;
using System.Linq;

namespace SocksCore
{
    public sealed class SocksCore : IClientConnectionsHandler
    {

        /*
            0x5a = запрос предоставлен
            0x5b = запрос отклонён или ошибочен
            0x5c = запрос не удался, потому что не запущен identd (или не доступен с сервера)
            0x5d = запрос не удался, поскольку клиентский identd не может подтвердить идентификатор пользователя в запросе
        */

        private const byte DefaultSocksError = (byte)Socks4ErrorCodes.Error;
        private enum Socks4ErrorCodes
        {
            Success = 0x5a,
            Error = 0x5b,
            NoIdent = 0x5c,
            InvalidLogin = 0x5d
        }

        private readonly ISocksClientHandler socks4ClientHandler;
        private readonly ISocksClientHandler socks5ClientHandler;


        public void AcceptClientConnection(ISocksClient client /*Socket client*/)
        {
            var socksVersionFromClientRequest = SocksVersion.Unknown;

            //var clientEndPoint = client.RemoteEndPoint as IPEndPoint;
            //if (clientEndPoint != null)
            //    OnClientConnected($"CC:{clientEndPoint.Address}");

            //socksVersionFromClientRequest = FromRequest(_picker);
            //OnClientRequestReceived($"CRR:{socksVersionFromClientRequest}");
            socksVersionFromClientRequest = FromRequest(client);



            switch (socksVersionFromClientRequest)
            {
                case SocksVersion.Unknown:
                    CloseConnectionAndSendError(client, DefaultSocksError);
                    break;
                case SocksVersion.Socks4:
                    socks4ClientHandler.HandleSocksRequest(client);
                    break;
                case SocksVersion.Socks5:
                    socks5ClientHandler.HandleSocksRequest(client);
                    break;
                default:
                    CloseConnectionAndSendError(client, DefaultSocksError);
                    break;
            }

        }

        private static SocksVersion FromRequest(IBytePicker picker)
        {
            var readedVersion = picker.PeekBytes(1);

            return (SocksVersion)readedVersion.First();
        }

        public SocksCore(ISocksClientHandler socks4ClientHandler, ISocksClientHandler socks5ClientHandler)
        {
            this.socks4ClientHandler = socks4ClientHandler;
            this.socks5ClientHandler = socks5ClientHandler;

        }

        public event EventHandler<string> ClientConnected;
        public event EventHandler<string> ClientRequestReceived;


        public void CloseConnectionAndSendError(ISocksClient connectionToClose, uint errorCode)
        {
            connectionToClose.Close();
            //            throw new NotImplementedException();
        }

        #region Events invokators

        //protected virtual void OnClientConnected(string e)
        //{
        //    ClientConnected?.Invoke(this, e);
        //}

        //protected virtual void OnClientRequestReceived(string e)
        //{
        //    ClientRequestReceived?.Invoke(this, e);
        //}



        #endregion

    }

    public interface ISocksClient : IBytePicker
    {

        void Close();


        void Send(byte[] errorArray);
    }
}

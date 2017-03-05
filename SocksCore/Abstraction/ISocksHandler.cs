using System.Net.Sockets;

namespace SocksCore
{
    public interface ISocksHandler
    {
        void HandleSocksRequest(TcpClient clientToHandle);
        void CloseConnectionAndSendError(TcpClient conectionToClose, uint errorCode);
    }

    public abstract class SocksHandler : ISocksHandler
    {

        public abstract void HandleSocksRequest(TcpClient clientToHandle);
        public void CloseConnectionAndSendError(TcpClient conectionToClose, uint errorCode)
        {
            throw new System.NotImplementedException();
        }
    }

    public abstract class Socks4Handler : SocksHandler
    {
        public override void HandleSocksRequest(TcpClient clientToHandle)
        {
            throw new System.NotImplementedException();
        }
    }
    public abstract class Socks5Handler : SocksHandler
    {

    }

}
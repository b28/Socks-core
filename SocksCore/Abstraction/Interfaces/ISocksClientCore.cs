using System;
using System.Net.Sockets;

namespace SocksCore
{
    public enum SocksVersion
    {
        Unknown,
        Socks4 = 0x04,
        Socks5 = 0x05
    }
    public interface ISocksClientCore : ISocksClientHandler
    {
        void AcceptClientConnection(Socket clientSocket);
        SocksVersion FromRequest(Socket requestSocket);
        event EventHandler<string> ClientRequestReceived;
        event EventHandler<string> ClientConnected;
    }


}
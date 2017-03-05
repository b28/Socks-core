using System.Net.Sockets;

namespace SocksCore
{
    public class ClientSocket : Socket
    {
        public ClientSocket(SocketType socketType, ProtocolType protocolType) : base(socketType, protocolType)
        {
        }

        public ClientSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) : base(addressFamily, socketType, protocolType)
        {
        }

        public ClientSocket(SocketInformation socketInformation) : base(socketInformation)
        {
        }
    }
}
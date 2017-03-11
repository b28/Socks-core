using System.Net.Sockets;
using System.Runtime.Serialization;

namespace SocksCore
{
    public class SocksException : SocketException
    {
        public SocksException()
        {
        }

        public SocksException(int errorCode) : base(errorCode)
        {
        }

        protected SocksException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}
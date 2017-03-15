using System.Runtime.Serialization;
using SocksCore.SocksHandlers;

namespace SocksCore
{
    public class ConnectionEstablisherException : SocksException
    {
        public ConnectionEstablisherException()
        {
        }

        public ConnectionEstablisherException(int errorCode) : base(errorCode)
        {
        }

        protected ConnectionEstablisherException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}
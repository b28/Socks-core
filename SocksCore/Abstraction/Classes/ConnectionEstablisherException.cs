using SocksCore.SocksHandlers;
using System.Net;
using System.Runtime.Serialization;

namespace SocksCore
{
    public class ConnectionEstablisherException : SocksException
    {
        public IPEndPoint EndPointToConnect { get; }
        public ConnectionEstablisherException()
        {

        }

        public ConnectionEstablisherException(IPEndPoint endPointToConnect)
        {
            EndPointToConnect = endPointToConnect;
        }

        public ConnectionEstablisherException(int errorCode) : base(errorCode)
        {
        }

        protected ConnectionEstablisherException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}
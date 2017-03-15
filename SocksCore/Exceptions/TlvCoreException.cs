using System;
using System.Runtime.Serialization;

namespace SocksCore
{
    public class TlvCoreException : Exception
    {
        public TlvCoreException()
        {

        }

        public TlvCoreException(string message) : base(message)
        {
        }

        public TlvCoreException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TlvCoreException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
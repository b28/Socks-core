using System;

namespace SocksTest
{
    public class ConnectorException : Exception
    {
        public ConnectorException()
        {
        }

        public ConnectorException(string message) : base(message)
        {

        }


        public ConnectorException(string message, Exception inner) : base(message, inner)
        {

        }
    }

    public class SocketConnectionException : ConnectorException
    {
        public SocketConnectionException()
        {
        }

        public SocketConnectionException(string message) : base(message)
        {

        }


        public SocketConnectionException(string message, Exception inner) : base(message, inner)
        {

        }
    }

    public class IncorrectProxyAuthorization : ConnectorException
    {
        public IncorrectProxyAuthorization()
        {
        }

        public IncorrectProxyAuthorization(string message) : base(message)
        {

        }


        public IncorrectProxyAuthorization(string message, Exception inner) : base(message, inner)
        {

        }
    }

    public class IncorrectMessage2Responce : ConnectorException
    {
        public IncorrectMessage2Responce()
        {
        }

        public IncorrectMessage2Responce(string message) : base(message)
        {

        }


        public IncorrectMessage2Responce(string message, Exception inner) : base(message, inner)
        {

        }
    }

    public class IncorrectServerChallenge : ConnectorException
    {
        public IncorrectServerChallenge()
        {
        }

        public IncorrectServerChallenge(string message) : base(message)
        {

        }


        public IncorrectServerChallenge(string message, Exception inner) : base(message, inner)
        {

        }
    }
}



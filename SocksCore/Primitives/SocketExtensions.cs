using System;
using System.Net.Sockets;

namespace SocksCore.Primitives
{
    public static class SocketExtensions
    {

        public delegate void ReadUntillSpecifiedByteReaded(Socket readFrom, out byte[] readTo, byte readToByte);

        private const int DefaultSocketsOperationsTimeout = 10000;

        private struct SocketTimeouts
        {
            public int ReceiveTimeout;
            public int SendTimeout;

            public static SocketTimeouts DefaultValues => new SocketTimeouts { ReceiveTimeout = DefaultSocketsOperationsTimeout, SendTimeout = DefaultSocketsOperationsTimeout };
        }
        private static SocketTimeouts SetSocketTimeouts(this Socket socket, SocketTimeouts timeouts)
        {
            var olValues = new SocketTimeouts
            {
                ReceiveTimeout = socket.ReceiveTimeout,
                SendTimeout = socket.SendTimeout
            };
            socket.ReceiveTimeout = timeouts.ReceiveTimeout;
            socket.SendTimeout = timeouts.SendTimeout;
            return olValues;
        }

        public static byte[] ReadComplete(this Socket readFromSocket, int bytesCount)
        {
            return readFromSocket.ReadComplete(bytesCount, SocketFlags.None);
        }

        /// <summary>
        /// This extension method guarantee reading from a socket needed amount of data bytes.
        /// If Not enought data to reading untill reading timeout, throwing SocketException.
        /// </summary>
        /// <param name="peekFromSocket">Socket to read from.</param>
        /// <param name="bytesCount">Bytes count that you wanna read</param>
        /// <param name="flagsToRead">Can peek for example.</param>
        /// <returns></returns>
        private static byte[] ReadComplete(this Socket peekFromSocket, int bytesCount, SocketFlags flagsToRead = SocketFlags.None)
        {
            var result = new byte[bytesCount];

            var temporarybuff = new byte[bytesCount];
            var alreadyReadedBytesCount = 0;
            do
            {
                var thisSessionReadedBytes = peekFromSocket.Receive(temporarybuff, bytesCount - alreadyReadedBytesCount, flagsToRead);
                if (thisSessionReadedBytes < 1)
                    throw new SocketException();
                Array.Copy(temporarybuff, 0, result, alreadyReadedBytesCount, thisSessionReadedBytes);
                alreadyReadedBytesCount += thisSessionReadedBytes;
            } while (alreadyReadedBytesCount < bytesCount);
            return result;
        }



        /// <summary>
        /// Peek <see cref="bytesCount"/> bytes from a socket with <see cref="timeout"/> timeout.
        /// </summary>
        /// <param name="socketToPeek">Socket to peek from</param>
        /// <param name="bytesCount">Bytes count to peek from socket</param>
        /// <returns>Bytes picked from socket.</returns>
        public static byte[] PeekBytes(this Socket socketToPeek, int bytesCount)
        {
            return socketToPeek.ReadComplete(bytesCount, SocketFlags.Peek);
        }




        public static void SetupSocketTimeouts(this Socket socket, SocketSettings settings)
        {

            socket.ReceiveTimeout = settings.NetworkClientReceiveTimeout;
            socket.SendTimeout = settings.NetworkClientSendTimeout;
            socket.ReceiveBufferSize = settings.NetworkClientReceiveBufferSize;
            socket.SendBufferSize = settings.NetworkClientSendBufferSize;
            socket.NoDelay = settings.UseNoDelay;



            NetworkHelper.SetKeepAliveValues(socket, true,
                 keepaLiveTime: settings.NetworkClientKeepAliveTimeout,
                 keepAliveInterval: settings.NetworkClientKeepAliveInterval);


        }

    }
}
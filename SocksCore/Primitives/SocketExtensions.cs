using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace SocksCore.Primitives
{
    public static class MarshalHelper
    {


        public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(),
                typeof(T));
            handle.Free();
            return stuff;
        }
        public static byte[] StructureToByteArray<T>(T obj) where T : struct
        {
            var len = Marshal.SizeOf(obj);
            var arr = new byte[len];
            var ptr = Marshal.AllocHGlobal(len);
            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, len);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }


    }

    public class SocketSettings
    {
        public uint NetworkClientKeepAliveInterval = 1000;

        public uint NetworkClientKeepAliveTimeout = 1000;

        public int NetworkClientReceiveBufferSize = 1024 * 1024 * 3;

        public int NetworkClientReceiveTimeout = 1000 * 60 * 180;

        public int NetworkClientSendBufferSize = 1024 * 1024 * 3;

        public int NetworkClientSendTimeout = 1000 * 60 * 180;

        public bool UseNoDelay = true;


        public SocketSettings()
        {

        }

        public static SocketSettings Default => new SocketSettings();


        public static SocketSettings DefaultHigh
        {
            get
            {
                var timeOut = 1000 * 60 * 30;
                return new SocketSettings()
                {
                    NetworkClientReceiveTimeout = timeOut,
                    NetworkClientSendTimeout = timeOut,
                    NetworkClientKeepAliveInterval = 1000,
                    UseNoDelay = true,
                    NetworkClientKeepAliveTimeout = 1000,
                };
            }
        }

    }
    public static class SocketExtensions
    {

        public delegate void ReadUntillSpecifiedByteReaded(Socket readFrom, out byte[] readTo, byte readToByte, int limit);

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
        /// <param name="timeout">Time period in milliseconds, what you ready to wait for data reading.</param>
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
        /// <param name="timeout">Timeout in milliseconds. (10000 default)</param>
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

    public static class NetworkHelper
    {
        public static bool SetKeepAliveValues(Socket socket, bool onOff, uint keepaLiveTime, uint keepAliveInterval)
        {
            var keepAliveValues = new TcpKeepAlive
            {
                OnOff = Convert.ToUInt32(onOff),
                KeepAliveTime = keepaLiveTime,
                KeepAliveInterval = keepAliveInterval,

            };


            try
            {
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, onOff);
                int result = socket.IOControl(IOControlCode.KeepAliveValues, MarshalHelper.StructureToByteArray(keepAliveValues),
                       null);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


        [StructLayout(LayoutKind.Sequential)]
        private struct TcpKeepAlive
        {
            public uint OnOff;
            public uint KeepAliveTime;
            public uint KeepAliveInterval;
        }


    }
}
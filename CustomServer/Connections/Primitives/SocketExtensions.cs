using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace CustomServer.Connections.Primitives
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
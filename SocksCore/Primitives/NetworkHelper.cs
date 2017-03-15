using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using SocksCore.Utils;

namespace SocksCore.Primitives
{
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
                int result = socket.IOControl(IOControlCode.KeepAliveValues, keepAliveValues.ToByteArray(),
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
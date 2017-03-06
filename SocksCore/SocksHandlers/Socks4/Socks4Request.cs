using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace SocksCore.SocksHandlers.Socks4
{
    public struct Socks4Request
    {
        public Socks4RequestHeader Header { get; private set; }
        public string UserName { get; private set; }
        public IPAddress IpAddress => new IPAddress(BitConverter.GetBytes(Header.IpAddress));

        private static string ReadUserName(IByteReceiver clientToHandle)
        {
            byte readedByte;
            var username = new List<byte>();

            do
            {
                readedByte = clientToHandle.Receive(1).First();
                if (readedByte == 0)
                    break;
                username.Add(readedByte);
            } while (true);
            return Encoding.ASCII.GetString(username.ToArray());
        }

        public static Socks4Request From(IByteReceiver receiver)
        {

            var structSize = Marshal.SizeOf(typeof(Socks4RequestHeader));

            var requestHeader =
                Socks4RequestHeaderFabric.FromHeader(receiver.Receive(structSize));

            var userName = ReadUserName(receiver);

            var socks4Request = new Socks4Request { Header = requestHeader, UserName = userName };
            return socks4Request;

        }

    }
}
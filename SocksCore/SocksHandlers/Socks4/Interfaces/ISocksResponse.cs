using SocksCore.Primitives;
using System.Runtime.InteropServices;
using SocksCore.Utils;

namespace SocksCore.SocksHandlers.Socks4
{
    //public interface ISocksResponse
    //{
    //    byte Tail1;// { get; }
    //    Socks4ErrorCodes ErrorCode { get; }
    //}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Socks4Response // ISocks4Response
    {

        public byte Tail1;// => 0x00;
        public Socks4ErrorCodes ErrorCode { get; }
        public ushort Tail2;// => 0x00;
        public uint Tail3;// => 0x00;

        public Socks4Response(Socks4ErrorCodes errorCode)
        {
            ErrorCode = errorCode;
            Tail3 = Tail2 = Tail1 = 0;
        }
        public byte[] GetBytes()
        {
            return this.ToByteArray();
        }
    }
}
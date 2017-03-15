using SocksCore.Primitives;
using SocksCore.Utils;
using System.Runtime.InteropServices;

namespace SocksCore.SocksHandlers.Socks4
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Socks4RequestHeader
    {
        public byte ProtocolVersion;
        public Socks4RequestType RequestType;//{ get; set; }
        public ushort Port;// { get; set; }
        public uint IpAddress;//{ get; set; }
    }

    public static class Socks4RequestHeaderFabric
    {
        public static Socks4RequestHeader FromHeader(byte[] fullHeader)
        {
            var result = fullHeader.ToStructure<Socks4RequestHeader>();
            result.Port = result.Port.SwapBytes();
            return result;
        }
    }
}
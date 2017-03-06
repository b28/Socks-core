using SocksCore.Primitives;
using System.Runtime.InteropServices;

namespace SocksCore.SocksHandlers.Socks4
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Socks4RequestHeader
    {
        public byte ProtocolVersion;
        public Socks4RequestType RequestType;//{ get; set; }
        public ushort Port;// { get; set; }
        public int IpAddress;//{ get; set; }
    }

    public static class Socks4RequestHeaderFabric
    {
        public static Socks4RequestHeader FromHeader(byte[] fullHeader)
        {
            return MarshalHelper.ByteArrayToStructure<Socks4RequestHeader>(fullHeader);
        }
    }
}
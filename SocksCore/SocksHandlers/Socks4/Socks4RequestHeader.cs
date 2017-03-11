using SocksCore.Primitives;
using System.Runtime.InteropServices;
using SocksCore.Utils;
using SocksCore.Utils.Log;

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
            var result = MarshalHelper.ByteArrayToStructure<Socks4RequestHeader>(fullHeader);
            result.Port = result.Port.SwapBytes();
            return result;
        }
    }
}
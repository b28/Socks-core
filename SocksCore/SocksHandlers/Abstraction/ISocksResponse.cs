using SocksCore.Primitives;
using System.Runtime.InteropServices;
using SocksCore.Utils;

namespace SocksCore.SocksHandlers
{
    public interface ISocksResponse
    {
        //TODO: very inefficient statement used "byte" should be replaced by another things in future releases because lack of knowledges about C#
        byte[] FormatResponse(byte errorCode);
    }


    public interface ISocks4Response : ISocksResponse
    {
        byte Header0 { get; }

        ushort Header1 { get; }
        uint Header2 { get; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Socks4Response : ISocks4Response
    {
        public byte Header0 => 0x00;
        public byte ResponseCode { get; private set; }
        public ushort Header1 => 0x00;
        public uint Header2 => 0x00;
        public byte[] FormatResponse(byte errorCode)
        {
            var r = new Socks4Response() { ResponseCode = errorCode };
            return r.ToByteArray();
        }

        
    }

    public enum Socks4ErrorCodes : byte
    {
        Success = 0x5a,
        Error = 0x5b,
        NoIdent = 0x5c,
        InvalidLogin = 0x5d
    }
}
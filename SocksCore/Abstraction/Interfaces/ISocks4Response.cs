namespace SocksCore
{
    public interface ISocks4Response
    {
        byte Header0 { get; }

        ushort Header1 { get; }
        uint Header2 { get; }
    }

    public struct Socks4Response : ISocks4Response
    {
        public byte Header0 { get; }
        public Socks4ErrorCodes ResponseCode { get; }
        public ushort Header1 { get; }
        public uint Header2 { get; }
        
    }

    public enum Socks4ErrorCodes : byte
    {
        Success = 0x5a,
        Error = 0x5b,
        NoIdent = 0x5c,
        InvalidLogin = 0x5d
    }
}
namespace SocksCore.SocksHandlers.Socks4
{
    public interface ISocksResponse
    {
        byte SocksVersion { get; }
        Socks4ErrorCodes ErrorCode { get; }
    }
}
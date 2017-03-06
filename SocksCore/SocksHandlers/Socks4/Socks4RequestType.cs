namespace SocksCore.SocksHandlers.Socks4
{
    public enum Socks4RequestType : byte
    {
        TcpIpConnection = 0x01,
        PortBinding = 0x02
    }
}
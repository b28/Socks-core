namespace SocksCore.Primitives
{
    public struct SocketTimeouts : ISocketTimeouts
    {
        public int ReceiveTimeout { get; set; }
        public int SendTimeout { get; set; }
    }
}
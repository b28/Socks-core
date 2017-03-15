namespace SocksCore.Primitives
{
    public interface ISocketTimeouts
    {
        int ReceiveTimeout { get; set; }
        int SendTimeout { get; set; }
    }
}
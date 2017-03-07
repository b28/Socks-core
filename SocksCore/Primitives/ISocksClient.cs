namespace SocksCore.Primitives
{
    public interface ISocksClient : IBytePeeker, IByteReceiver
    {
        void Close();
        void Send(byte[] errorArray);
    }
}
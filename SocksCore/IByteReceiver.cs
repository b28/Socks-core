namespace SocksCore
{
    public interface IByteReceiver
    {
        byte[] Receive(int bytesCount);
    }
}
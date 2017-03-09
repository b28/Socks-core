namespace SocksCore.Utils.Log
{
    public interface ICanLog
    {
        void Fatal(string msg);
        void Debug(string msg);
        void Error(string msg);
        void Notice(string msg);
        void Trace(string msg);
        void Warning(string msg);
    }
}
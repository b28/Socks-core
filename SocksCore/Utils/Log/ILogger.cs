namespace SocksCore.Utils.Log
{
    public interface ILogger : ICanLog
    {
        Logger.LogLevel CurrentLogLevel { get; set; }
        void LogMsg(string msg, Logger.LogLevel logLevel = Logger.LogLevel.Error);
    }
}
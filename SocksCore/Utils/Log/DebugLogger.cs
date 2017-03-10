using System;

namespace SocksCore.Utils.Log
{
    public class DebugLogger : ILogger
    {
        public void Fatal(string msg)
        {
            LogMsg($"Fatal: {msg}", Logger.LogLevel.Fatal);
        }

        public void Debug(string msg)
        {
            LogMsg($"Debug: {msg}", Logger.LogLevel.Debug);
        }

        public void Error(string msg)
        {
            LogMsg($"Error: {msg}", Logger.LogLevel.Error);
        }

        public void Notice(string msg)
        {
            LogMsg($"Notice: {msg}", Logger.LogLevel.Notice);
        }

        public void Trace(string msg)
        {
            LogMsg($"Trace: {msg}", Logger.LogLevel.Trace);
        }

        public void Warning(string msg)
        {
            LogMsg($"Warning: {msg}", Logger.LogLevel.Warning);
        }

        public Logger.LogLevel CurrentLogLevel { get; set; }
        public void LogMsg(string msg, Logger.LogLevel logLevel = Logger.LogLevel.Error)
        {
            if ((int)CurrentLogLevel < (int)logLevel)
                return;
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now.ToString("O")} {msg}");
        }
    }
}

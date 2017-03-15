using System;
using System.IO;
using System.Text;

namespace SocksCore.Utils.Log
{
    public sealed class Logger : ILogger
    {
        public const LogLevel DefaultLogLevel = LogLevel.Error;

        private readonly object locker = new object();
        private readonly string logFileName;

        public enum LogLevel : byte
        {
            Fatal = 0,
            Error,
            Warning,
            Notice,
            Trace,
            Debug,
            None
        }

        public Logger(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
                logFileName = fileName;
        }

        public LogLevel CurrentLogLevel { get; set; } = DefaultLogLevel;

        #region Log Methods
        public void Fatal(string msg)
        {
            LogMsg($"[Fatal]: {msg}", Logger.LogLevel.Fatal);
        }

        public void Debug(string msg)
        {
            LogMsg($"[Debug]: {msg}", Logger.LogLevel.Debug);
        }

        public void Error(string msg)
        {
            LogMsg($"[Error]: {msg}", Logger.LogLevel.Error);
        }

        public void Notice(string msg)
        {
            LogMsg($"[Notice]: {msg}", Logger.LogLevel.Notice);
        }

        public void Trace(string msg)
        {
            LogMsg($"[Trace]: {msg}", Logger.LogLevel.Trace);
        }

        public void Warning(string msg)
        {
            LogMsg($"[Warning]: {msg}", Logger.LogLevel.Warning);
        }
        #endregion
        public void LogMsg(string msg, LogLevel logLevel = DefaultLogLevel)
        {

            Console.WriteLine(msg);


            if ((int)CurrentLogLevel < (int)logLevel)
                return;
            lock (locker)
            {
                using (FileStream file = new FileStream(logFileName, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
                {
                    writer.Write($"{DateTime.Now.ToString("O")} {msg}{Environment.NewLine}");
                }
            }
        }


    }

}
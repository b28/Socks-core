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
            Fatal,
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
        public void Notice(string msg)
        {
            LogMsg(msg, LogLevel.Notice);
        }

        public void Warning(string msg)
        {
            LogMsg(msg, LogLevel.Warning);
        }

        public void Error(string msg)
        {
            LogMsg(msg, LogLevel.Error);
        }
        public void Debug(string msg)
        {
            LogMsg(msg, LogLevel.Debug);
        }
        public void Trace(string msg)
        {
            LogMsg(msg, LogLevel.Trace);
        }
        public void Fatal(string msg)
        {
            LogMsg(msg, LogLevel.Fatal);
        }
        #endregion
        public void LogMsg(string msg, LogLevel logLevel = DefaultLogLevel)
        {
            if (CurrentLogLevel >= logLevel)
                return;
            lock (locker)
            {
                using (FileStream file = new FileStream(logFileName, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
                {
                    writer.Write(msg + Environment.NewLine);
                }
            }
        }


    }

}
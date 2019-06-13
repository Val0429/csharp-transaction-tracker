using System;
using System.Diagnostics;

namespace Constant
{
    public class Logger : ILogger
    {
        private static volatile Logger _instance;
        private static readonly object SyncRoot = new object();

        private Logger()
        {
        }

        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Logger();
                        }
                    }
                }

                return _instance;
            }
        }

        public void Log(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    DebugFormat(message);
                    break;
                case LogLevel.Info:
                    InfoFormat(message);
                    break;
                case LogLevel.Warn:
                    WarnFormat(message);
                    break;
                case LogLevel.Error:
                    ErrorFormat(message);
                    break;
                case LogLevel.Fatal:
                    FatalFormat(message);
                    break;
            }
        }

        public void Debug(object message)
        {
            Trace.WriteLine(message);
        }

        public void Debug(object message, Exception exception)
        {
            Trace.WriteLine(string.Format("{0}\r\n{1}", message, exception));
        }

        public void DebugFormat(string message, params object[] args)
        {
            Trace.WriteLine(string.Format(message, args));
        }

        public void Info(object message)
        {
            Trace.WriteLine(message);
        }

        public void Info(object message, Exception exception)
        {
            Trace.WriteLine(string.Format("{0}\r\n{1}", message, exception));
        }

        public void InfoFormat(string message, params object[] args)
        {
            Trace.WriteLine(string.Format(message, args));
        }

        public void Warn(object message)
        {
            Trace.WriteLine(message);
        }

        public void Warn(object message, Exception exception)
        {
            Trace.WriteLine(string.Format("{0}\r\n{1}", message, exception));
        }

        public void WarnFormat(string message, params object[] args)
        {
            Trace.WriteLine(string.Format(message, args));
        }

        public void Error(object message)
        {
            Trace.WriteLine(message);
        }

        public void Error(object message, Exception exception)
        {
            Trace.WriteLine(string.Format("{0}\r\n{1}", message, exception));
        }

        public void ErrorFormat(string message, params object[] args)
        {
            Trace.WriteLine(string.Format(message, args));
        }

        public void Fatal(object message)
        {
            Trace.WriteLine(message);
        }

        public void Fatal(object message, Exception exception)
        {
            Trace.WriteLine(string.Format("{0}\r\n{1}", message, exception));
        }

        public void FatalFormat(string message, params object[] args)
        {
            Trace.WriteLine(string.Format(message, args));
        }
    }
}

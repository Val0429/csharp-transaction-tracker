using System;

namespace Constant
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);

        void Debug(object message);
        void Debug(object message, Exception exception);
        void DebugFormat(string message, params object[] args);

        void Info(object message);
        void Info(object message, Exception exception);
        void InfoFormat(string message, params object[] args);

        void Warn(object message);
        void Warn(object message, Exception exception);
        void WarnFormat(string message, params object[] args);

        void Error(object message);
        void Error(object message, Exception exception);
        void ErrorFormat(string message, params object[] args);

        void Fatal(object message);
        void Fatal(object message, Exception exception);
        void FatalFormat(string message, params object[] args);
    }

    public enum LogLevel { Debug, Info, Warn, Error, Fatal }
}
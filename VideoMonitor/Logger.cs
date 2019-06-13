using Constant;

namespace VideoMonitor
{
    class Logger
    {
        private static ILogger _current;
        public static ILogger Current { get { return _current ?? (_current = LoggerManager.Instance.GetLogger()); } }

        private Logger()
        {

        }
    }
}
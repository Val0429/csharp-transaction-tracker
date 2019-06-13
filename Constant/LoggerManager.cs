using System;
using System.Reflection;
using Constant.Utility;

namespace Constant
{
    public class LoggerManager
    {
        private static volatile LoggerManager _instance;
        private static readonly object SyncRoot = new object();


        private readonly ILoggerFactory _factory;

        private LoggerManager()
        {
            try
            {
                _factory = ActivatorUtility.Create<ILoggerFactory>("loggerActivator");

                if (_factory == null) _factory = new DefaultLoggerFactory();
            }
            catch (Exception ex)
            {
                // default logger
                var logger = Constant.Logger.Instance;

                logger.Error(ex);

                _factory = new DefaultLoggerFactory();
            }
        }

        public static LoggerManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new LoggerManager();
                        }
                    }
                }

                return _instance;
            }
        }


        // Method
        public ILogger GetLogger()
        {
            try
            {
                var callingAssembly = Assembly.GetCallingAssembly();
                var assemblyName = callingAssembly.GetName().Name;
                var logger = _factory.Create(assemblyName);

                return logger;
            }
            catch (Exception ex)
            {
                // default logger
                var logger = Constant.Logger.Instance;

                logger.Error(ex);

                return logger;
            }
        }

        public ILogger GetLogger(string loggerName)
        {
            try
            {
                var logger = _factory.Create(loggerName);

                return logger;
            }
            catch (Exception ex)
            {
                // default logger
                var logger = Constant.Logger.Instance;

                logger.Error(ex);

                return logger;
            }
        }
    }
}
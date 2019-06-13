namespace Constant
{
    public interface ILoggerFactory
    {
        ILogger Create();

        ILogger Create(string name);
    }

    public class DefaultLoggerFactory : ILoggerFactory
    {
        private readonly ILogger _logger = Constant.Logger.Instance;


        public ILogger Create()
        {
            return _logger;
        }

        public ILogger Create(string name)
        {
            return _logger;
        }
    }
}
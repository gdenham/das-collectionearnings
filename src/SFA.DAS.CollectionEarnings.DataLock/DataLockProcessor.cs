using NLog;

namespace SFA.DAS.CollectionEarnings.DataLock
{
    internal class DataLockProcessor
    {
        private readonly ILogger _logger;

        public DataLockProcessor(ILogger logger)
        {
            _logger = logger;
        }

        public void Process()
        {
            _logger.Info("Started Data Lock Processor.");
            _logger.Info("Finished Data Lock Processor.");
        }
    }
}

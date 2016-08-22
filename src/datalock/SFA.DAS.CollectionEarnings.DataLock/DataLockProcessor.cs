using NLog;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.Data.Repositories;

namespace SFA.DAS.CollectionEarnings.DataLock
{
    internal class DataLockProcessor
    {
        private readonly ILogger _logger;
        private readonly IValidationErrorRepository _validationErrorRepository;

        public DataLockProcessor(ILogger logger, IValidationErrorRepository validationErrorRepository)
        {
            _logger = logger;
            _validationErrorRepository = validationErrorRepository;
        }

        public void Process()
        {
            _logger.Info("Started Data Lock Processor.");

            _logger.Debug("Started writing validation error.");
            var validationError = new ValidationError()
            {
                LearnRefNumber = "Lrn001",
                AimSeqNumber = 1,
                RuleId = "DLOCK_01"
            };

            _validationErrorRepository.AddValidationError(validationError);
            _logger.Debug("Finished writing validation error.");

            _logger.Info("Finished Data Lock Processor.");
        }
    }
}

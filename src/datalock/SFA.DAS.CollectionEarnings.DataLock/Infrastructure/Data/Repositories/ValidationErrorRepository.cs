using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Repositories
{
    public class ValidationErrorRepository : DcfsRepository, IValidationErrorRepository
    {
        private const string ValidationErrorDestination = "DataLock.ValidationError";
        private const string AddValidationErrorCommand = "INSERT INTO " + ValidationErrorDestination +
                                                        " (Ukprn, LearnRefNumber, AimSeqNumber, RuleId, PriceEpisodeIdentifier,EpisodeStartDate)" +
                                                        " VALUES (@Ukprn, @LearnRefNumber, @AimSeqNumber, @RuleId,@PriceEpisodeIdentifier,@EpisodeStartDate)";

        public ValidationErrorRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void AddValidationError(ValidationErrorEntity validationError)
        {
            Execute(AddValidationErrorCommand, new
            {
                Ukprn = validationError.Ukprn,
                LearnRefNumber = validationError.LearnRefNumber,
                AimSeqNumber = validationError.AimSeqNumber,
                RuleId = validationError.RuleId,
                PriceEpisodeIdentifier = validationError.PriceEpisodeIdentifier,
                EpisodeStartDate = validationError.EpisodeStartDate
            });
        }

        public void AddValidationErrors(ValidationErrorEntity[] validationErrors)
        {
            ExecuteBatch(validationErrors, ValidationErrorDestination);
        }
    }
}

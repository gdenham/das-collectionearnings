using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Repositories
{
    public class ProcessedLearningDeliveryPeriodRepository : DcfsRepository, IProcessedLearningDeliveryPeriodRepository
    {
        private const string LearningDeliveryPeriodDestination = "Rulebase.AE_LearningDelivery_Period";

        public ProcessedLearningDeliveryPeriodRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void AddProcessedLearningDeliveryPeriod(ProcessedLearningDeliveryPeriod[] periodEarnings)
        {
            ExecuteBatch(periodEarnings, LearningDeliveryPeriodDestination);
        }
    }
}
using SFA.DAS.CollectionEarnings.Calculator.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.Calculator.Data.Repositories
{
    public class ProcessedLearningDeliveryPeriodisedValuesRepository : DcfsRepository, IProcessedLearningDeliveryPeriodisedValuesRepository
    {
        private const string LearningDeliveryPeriodisedValuesDestination = "Rulebase.AE_LearningDelivery_PeriodisedValues";

        public ProcessedLearningDeliveryPeriodisedValuesRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void AddProcessedLearningDeliveryPeriodisedValues(ProcessedLearningDeliveryPeriodisedValues[] periodisedValues)
        {
            ExecuteBatch(periodisedValues, LearningDeliveryPeriodisedValuesDestination);
        }
    }
}
using SFA.DAS.CollectionEarnings.Calculator.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.Calculator.Data.Repositories
{
    public class ProcessedLearningDeliveryRepository : DcfsRepository, IProcessedLearningDeliveryRepository
    {
        private const string LearningDeliveryDestination = "Rulebase.AE_LearningDelivery";

        public ProcessedLearningDeliveryRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void AddProcessedLearningDeliveries(ProcessedLearningDelivery[] deliveries)
        {
            ExecuteBatch(deliveries, LearningDeliveryDestination);
        }
    }
}
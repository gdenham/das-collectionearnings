using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.Calculator.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.Data.Repositories
{
    public interface IProcessedLearningDeliveryRepository
    {
        void AddProcessedLearningDeliveries(IEnumerable<ProcessedLearningDelivery> deliveries);
    }
}
using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.Calculator.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.Data.Repositories
{
    public interface ILearningDeliveryToProcessRepository
    {
        IEnumerable<LearningDeliveryToProcess> GetAllLearningDeliveriesToProcess();
    }
}
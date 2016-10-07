using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data
{
    public interface IProcessedLearningDeliveryPeriodisedValuesRepository
    {
        void AddProcessedLearningDeliveryPeriodisedValues(ProcessedLearningDeliveryPeriodisedValues[] periodisedValues);
    }
}
using MediatR;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDeliveryPeriodisedValues.AddProcessedLearningDeliveryPeriodisedValuesCommand
{
    public class AddProcessedLearningDeliveryPeriodisedValuesCommandRequest : IRequest
    {
         public Infrastructure.Data.Entities.ProcessedLearningDeliveryPeriodisedValues[] PeriodisedValues { get; set; }
    }
}
using MediatR;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDeliveryPeriod.AddProcessedLearningDeliveryPeriodCommand
{
    public class AddProcessedLearningDeliveryPeriodCommandRequest : IRequest
    {
         public LearningDeliveryPeriodEarning[] PeriodEarnings { get; set; }
    }
}
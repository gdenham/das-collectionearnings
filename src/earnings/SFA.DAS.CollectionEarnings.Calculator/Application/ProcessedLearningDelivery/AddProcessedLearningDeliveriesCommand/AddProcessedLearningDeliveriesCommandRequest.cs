using MediatR;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDelivery.AddProcessedLearningDeliveriesCommand
{
    public class AddProcessedLearningDeliveriesCommandRequest : IRequest
    {
         public Infrastructure.Data.Entities.ProcessedLearningDelivery[] LearningDeliveries { get; set; }
    }
}
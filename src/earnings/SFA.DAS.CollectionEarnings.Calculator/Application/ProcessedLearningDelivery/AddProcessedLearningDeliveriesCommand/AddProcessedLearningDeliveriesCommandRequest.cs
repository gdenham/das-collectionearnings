using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDelivery.AddProcessedLearningDeliveriesCommand
{
    public class AddProcessedLearningDeliveriesCommandRequest : IRequest<AddProcessedLearningDeliveriesCommandResponse>
    {
         public IEnumerable<Data.Entities.ProcessedLearningDelivery> LearningDeliveries { get; set; }
    }
}
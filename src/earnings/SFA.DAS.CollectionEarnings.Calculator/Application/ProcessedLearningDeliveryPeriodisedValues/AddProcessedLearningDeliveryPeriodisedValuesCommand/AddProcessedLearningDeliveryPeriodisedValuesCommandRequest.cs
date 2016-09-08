using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDeliveryPeriodisedValues.AddProcessedLearningDeliveryPeriodisedValuesCommand
{
    public class AddProcessedLearningDeliveryPeriodisedValuesCommandRequest : IRequest<AddProcessedLearningDeliveryPeriodisedValuesCommandResponse>
    {
         public IEnumerable<Data.Entities.ProcessedLearningDeliveryPeriodisedValues> PeriodisedValues { get; set; }
    }
}
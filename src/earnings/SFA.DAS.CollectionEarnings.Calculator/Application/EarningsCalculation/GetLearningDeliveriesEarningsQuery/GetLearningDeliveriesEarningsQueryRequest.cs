using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery
{
    public class GetLearningDeliveriesEarningsQueryRequest : IRequest<GetLearningDeliveriesEarningsQueryResponse>
    {
         public IEnumerable<Data.Entities.LearningDeliveryToProcess> LearningDeliveries { get; set; } 
    }
}
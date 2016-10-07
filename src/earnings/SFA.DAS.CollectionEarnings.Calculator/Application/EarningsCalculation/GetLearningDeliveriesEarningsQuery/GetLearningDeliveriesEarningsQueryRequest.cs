using MediatR;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery
{
    public class GetLearningDeliveriesEarningsQueryRequest : IRequest<GetLearningDeliveriesEarningsQueryResponse>
    {
         public Infrastructure.Data.Entities.LearningDeliveryToProcess[] LearningDeliveries { get; set; } 
    }
}
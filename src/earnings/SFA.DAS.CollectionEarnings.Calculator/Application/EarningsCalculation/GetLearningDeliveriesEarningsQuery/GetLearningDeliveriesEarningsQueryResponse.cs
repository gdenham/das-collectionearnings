using SFA.DAS.Payments.DCFS.Application;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery
{
    public class GetLearningDeliveriesEarningsQueryResponse : Response
    {
        public Infrastructure.Data.Entities.ProcessedLearningDelivery[] ProcessedLearningDeliveries { get; set; }
        public Infrastructure.Data.Entities.ProcessedLearningDeliveryPeriodisedValues[] ProcessedLearningDeliveryPeriodisedValues { get; set; }
    }
}
using System.Collections.Generic;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery
{
    public class GetLearningDeliveriesEarningsQueryResponse : Response
    {
        public IEnumerable<Data.Entities.ProcessedLearningDelivery> ProcessedLearningDeliveries { get; set; }
        public IEnumerable<Data.Entities.ProcessedLearningDeliveryPeriodisedValues> ProcessedLearningDeliveryPeriodisedValues { get; set; }
    }
}
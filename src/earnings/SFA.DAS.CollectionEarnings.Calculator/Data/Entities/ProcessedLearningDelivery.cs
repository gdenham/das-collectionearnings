using System;

namespace SFA.DAS.CollectionEarnings.Calculator.Data.Entities
{
    public class ProcessedLearningDelivery
    {
        public int Ukprn { get; set; }
        public string LearnRefNumber { get; set; }
        public long Uln { get; set; }
        public string NiNumber { get; set; }
        public int AimSeqNumber { get; set; }
        public long? StdCode { get; set; }
        public int? ProgType { get; set; }
        public int? FworkCode { get; set; }
        public int? PwayCode { get; set; }
        public int NegotiatedPrice { get; set; }
        public DateTime LearnStartDate { get; set; }
        public DateTime? OrigLearnStartDate { get; set; }
        public DateTime LearnPlanEndDate { get; set; }
        public DateTime? LearnActEndDate { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public decimal MonthlyInstallmentUncapped { get; set; }
        public decimal CompletionPayment { get; set; }
        public decimal CompletionPaymentUncapped { get; set; }
    }
}
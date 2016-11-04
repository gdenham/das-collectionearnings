namespace SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities
{
    public class ProcessedLearningDeliveryPeriod
    {
        public string LearnRefNumber { get; set; }
        public int AimSeqNumber { get; set; }
        public int Period { get; set; }
        public decimal ProgrammeAimOnProgPayment { get; set; }
        public decimal ProgrammeAimCompletionPayment { get; set; }
        public decimal ProgrammeAimBalPayment { get; set; }
    }
}
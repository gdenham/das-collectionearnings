using SFA.DAS.CollectionEarnings.Calculator.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.Calculator.Data.Repositories
{
    public class LearningDeliveryToProcessRepository : DcfsRepository, ILearningDeliveryToProcessRepository
    {
        private const string LearningDeliveryToProcessSource = "Rulebase.vw_AE_LearningDeliveriesToProcess";
        private const string LearningDeliveryToProcessColumns = "Ukprn," +
                                                                "LearnRefNumber," +
                                                                "Uln," +
                                                                "NiNumber," +
                                                                "AimSeqNumber," +
                                                                "StandardCode," +
                                                                "ProgrammeType," +
                                                                "FrameworkCode," +
                                                                "PathwayCode," +
                                                                "LearnStartDate," +
                                                                "OrigLearnStartDate," +
                                                                "LearnPlanEndDate," +
                                                                "LearnActEndDate," +
                                                                "CompletionStatus," +
                                                                "NegotiatedPrice";
        private const string SelectLearningDeliveriesToProcess = "SELECT " + LearningDeliveryToProcessColumns + " FROM " + LearningDeliveryToProcessSource;

        public LearningDeliveryToProcessRepository(string connectionString)
            : base(connectionString)
        {
        }

        public LearningDeliveryToProcess[] GetAllLearningDeliveriesToProcess()
        {
            return Query<LearningDeliveryToProcess>(SelectLearningDeliveriesToProcess);
        }
    }
}
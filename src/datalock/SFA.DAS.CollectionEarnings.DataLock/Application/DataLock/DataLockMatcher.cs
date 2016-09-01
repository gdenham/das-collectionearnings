namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock
{
    public class DataLockMatcher
    {
        public static bool MatchUkprn(Data.Entities.Commitment commitment, Data.Entities.DasLearner learner)
        {
            return commitment.Ukprn == learner.Ukprn;
        }

        public static bool MatchUln(Data.Entities.Commitment commitment, Data.Entities.DasLearner learner)
        {
            return learner.Uln.HasValue &&
                   commitment.Uln == learner.Uln.Value;
        }

        public static bool MatchStandard(Data.Entities.Commitment commitment, Data.Entities.DasLearner learner)
        {
            return learner.StdCode.HasValue &&
                   commitment.StandardCode.HasValue &&
                   commitment.StandardCode.Value == learner.StdCode.Value;
        }

        public static bool MatchFramework(Data.Entities.Commitment commitment, Data.Entities.DasLearner learner)
        {
            return commitment.FrameworkCode.HasValue &&
                   learner.FworkCode.HasValue &&
                   commitment.FrameworkCode.Value == learner.FworkCode.Value;
        }

        public static bool MatchProgramme(Data.Entities.Commitment commitment, Data.Entities.DasLearner learner)
        {
            return commitment.ProgrammeType.HasValue &&
                   learner.ProgType.HasValue &&
                   commitment.ProgrammeType.Value == learner.ProgType.Value;
        }

        public static bool MatchPathway(Data.Entities.Commitment commitment, Data.Entities.DasLearner learner)
        {
            return commitment.PathwayCode.HasValue &&
                   learner.PwayCode.HasValue &&
                   commitment.PathwayCode.Value == learner.PwayCode.Value;
        }

        public static bool MatchPrice(Data.Entities.Commitment commitment, Data.Entities.DasLearner learner)
        {
            return learner.TbFinAmount.HasValue &&
                   (long) commitment.AgreedCost == learner.TbFinAmount.Value;
        }
    }
}
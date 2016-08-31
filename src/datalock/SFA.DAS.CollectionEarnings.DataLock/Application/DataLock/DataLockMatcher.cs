namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock
{
    public class DataLockMatcher
    {
        public static bool Match(Data.Entities.Commitment commitment, Data.Entities.DasLearner learner, DataLockMatchLevel level)
        {
            // Ukprn match
            var result = commitment.Ukprn == learner.Ukprn;

            if (!result || level == DataLockMatchLevel.Ukprn)
            {
                return result;
            }

            // Uln match
            result = learner.Uln.HasValue && commitment.Uln == learner.Uln.Value;

            if (!result || level == DataLockMatchLevel.Uln)
            {
                return result;
            }

            if (learner.StdCode.HasValue)
            {
                // Standard match
                result = commitment.StandardCode.HasValue &&
                         commitment.StandardCode.Value == learner.StdCode.Value;

                if (!result || level == DataLockMatchLevel.Standard)
                {
                    return result;
                }
            }
            else
            {
                // Framework match
                result = commitment.FrameworkCode.HasValue &&
                   learner.FworkCode.HasValue &&
                   commitment.FrameworkCode.Value == learner.FworkCode.Value;

                if (!result || level == DataLockMatchLevel.Framework)
                {
                    return result;
                }

                // Programme match
                result = commitment.ProgrammeType.HasValue &&
                   learner.ProgType.HasValue &&
                   commitment.ProgrammeType.Value == learner.ProgType.Value;

                if (!result || level == DataLockMatchLevel.Programme)
                {
                    return result;
                }

                // Pathway match
                result = commitment.PathwayCode.HasValue &&
                   learner.PwayCode.HasValue &&
                   commitment.PathwayCode.Value == learner.PwayCode.Value;

                if (!result || level == DataLockMatchLevel.Pathway)
                {
                    return result;
                }
            }

            // Price match
            result = learner.TbFinAmount.HasValue &&
                     (long)commitment.AgreedCost == learner.TbFinAmount.Value;

            return result;
        }
    }
}
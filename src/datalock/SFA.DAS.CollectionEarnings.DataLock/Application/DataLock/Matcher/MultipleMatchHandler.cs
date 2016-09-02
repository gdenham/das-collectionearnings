using System.Collections.Generic;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class MultipleMatchHandler : MatchHandler
    {
        public override string Match(List<Data.Entities.Commitment> commitments, Data.Entities.DasLearner learner)
        {
            if (commitments.Count > 1)
            {
                return DataLockErrorCodes.MultipleMatches;
            }

            return null;
        }
    }
}
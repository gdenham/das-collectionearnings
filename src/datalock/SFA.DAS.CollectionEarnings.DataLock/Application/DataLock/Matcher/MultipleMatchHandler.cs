using System.Collections.Generic;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class MultipleMatchHandler : MatchHandler
    {
        public override string Match(List<Infrastructure.Data.Entities.CommitmentEntity> commitments, Infrastructure.Data.Entities.LearnerEntity learner)
        {
            if (commitments.Count > 1)
            {
                return DataLockErrorCodes.MultipleMatches;
            }

            return ExecuteNextHandler(commitments, learner);
        }
    }
}
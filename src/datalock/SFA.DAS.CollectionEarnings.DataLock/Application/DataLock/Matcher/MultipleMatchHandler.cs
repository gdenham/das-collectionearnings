using System.Collections.Generic;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class MultipleMatchHandler : MatchHandler
    {
        public override MatchResult Match(List<Commitment.Commitment> commitments, Infrastructure.Data.Entities.LearnerEntity learner)
        {
            if (commitments.Count > 1)
            {
                return new MatchResult
                {
                    ErrorCode = DataLockErrorCodes.MultipleMatches
                };
            }

            return ExecuteNextHandler(commitments, learner);
        }
    }
}
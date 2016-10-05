using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class StandardMatchHandler : MatchHandler
    {
        public override MatchResult Match(List<Infrastructure.Data.Entities.CommitmentEntity> commitments, Infrastructure.Data.Entities.LearnerEntity learner)
        {
            if (learner.StandardCode.HasValue)
            {
                var commitmentsToMatch = commitments.Where(c => c.StandardCode.HasValue &&
                                                                c.StandardCode.Value == learner.StandardCode.Value).ToList();

                if (commitmentsToMatch.Any())
                {
                    return ExecuteNextHandler(commitmentsToMatch, learner);
                }

                return new MatchResult
                {
                    ErrorCode = DataLockErrorCodes.MismatchingStandard
                };
            }

            return ExecuteNextHandler(commitments, learner);
        }
    }
}
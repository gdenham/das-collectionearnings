using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class FrameworkMatchHandler : MatchHandler
    {
        public override MatchResult Match(List<Infrastructure.Data.Entities.CommitmentEntity> commitments, Infrastructure.Data.Entities.LearnerEntity learner)
        {
            if (!learner.StandardCode.HasValue)
            {
                var commitmentsToMatch = commitments.Where(c => c.FrameworkCode.HasValue &&
                                                                learner.FrameworkCode.HasValue &&
                                                                c.FrameworkCode.Value == learner.FrameworkCode.Value)
                    .ToList();

                if (commitmentsToMatch.Any())
                {
                    return ExecuteNextHandler(commitmentsToMatch, learner);
                }

                return new MatchResult
                {
                    ErrorCode = DataLockErrorCodes.MismatchingFramework
                };
            }

            return ExecuteNextHandler(commitments, learner);
        }
    }
}
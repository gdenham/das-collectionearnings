using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class PathwayMatchHandler : MatchHandler
    {
        public override MatchResult Match(List<Commitment.Commitment> commitments, Learner.Learner learner)
        {
            if (!learner.StandardCode.HasValue)
            {
                var commitmentsToMatch = commitments.Where(c => c.PathwayCode.HasValue &&
                                                                learner.PathwayCode.HasValue &&
                                                                c.PathwayCode.Value == learner.PathwayCode.Value).ToList();

                if (commitmentsToMatch.Any())
                {
                    return ExecuteNextHandler(commitmentsToMatch, learner);
                }

                return new MatchResult
                {
                    ErrorCode = DataLockErrorCodes.MismatchingPathway
                };
            }

            return ExecuteNextHandler(commitments, learner);
        }
    }
}
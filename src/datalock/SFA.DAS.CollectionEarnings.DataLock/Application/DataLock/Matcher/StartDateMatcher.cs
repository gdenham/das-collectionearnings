using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class StartDateMatcher : MatchHandler
    {
        public override MatchResult Match(List<Commitment.Commitment> commitments, Learner.Learner learner)
        {
            var commitmentsToMatch = commitments.Where(c => learner.LearningStartDate >= c.StartDate
                                                            && learner.LearningStartDate < c.EndDate).ToList();

            if (!commitmentsToMatch.Any())
            {
                return new MatchResult
                {
                    ErrorCode = DataLockErrorCodes.EarlierStartDate
                };
            }

            return ExecuteNextHandler(commitmentsToMatch, learner);
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class StartDateMatcher : MatchHandler
    {
        public override MatchResult Match(List<Commitment.Commitment> commitments, Learner.Learner learner)
        {
            var commitmentsToMatch = commitments.Where(c => learner.LearningStartDate.HasValue
                                                            && learner.LearningStartDate.Value >= c.StartDate
                                                            && learner.LearningStartDate.Value < c.EndDate).ToList();

            if (!commitmentsToMatch.Any())
            {
                return new MatchResult
                {
                    ErrorCode = DataLockErrorCodes.EarlierStartMonth
                };
            }

            return ExecuteNextHandler(commitmentsToMatch, learner);
        }
    }
}
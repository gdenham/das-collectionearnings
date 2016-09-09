using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class StartMonthMatcher : MatchHandler
    {
        public override string Match(List<Data.Entities.Commitment> commitments, Data.Entities.DasLearner learner)
        {
            var commitmentsToMatch = commitments.Where(c => learner.LearnStartDate.HasValue &&
                                                            (learner.LearnStartDate.Value.Month - c.StartDate.Month 
                                                            + 12 * (learner.LearnStartDate.Value.Year - c.StartDate.Year) >= 0)).ToList();

            if (!commitmentsToMatch.Any())
            {
                return DataLockErrorCodes.EarlierStartMonth;
            }
            else
            {
                return ExecuteNextHandler(commitmentsToMatch, learner);
            }
        }
    }
}
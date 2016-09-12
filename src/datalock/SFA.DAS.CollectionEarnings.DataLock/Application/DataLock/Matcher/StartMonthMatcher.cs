using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Tools.Extensions;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class StartMonthMatcher : MatchHandler
    {
        public override string Match(List<Data.Entities.Commitment> commitments, Data.Entities.DasLearner learner)
        {
            var commitmentsToMatch = commitments.Where(c => learner.LearnStartDate.HasValue &&
                                                            learner.LearnStartDate.Value.FirstDayOfMonth() >= c.StartDate.FirstDayOfMonth()).ToList();

            if (!commitmentsToMatch.Any())
            {
                return DataLockErrorCodes.EarlierStartMonth;
            }

            return ExecuteNextHandler(commitmentsToMatch, learner);
        }
    }
}
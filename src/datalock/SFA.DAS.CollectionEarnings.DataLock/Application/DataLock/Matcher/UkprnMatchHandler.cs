using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class UkprnMatchHandler : MatchHandler
    {
        public override string Match(List<Data.Entities.Commitment> commitments, Data.Entities.DasLearner learner)
        {
            var commitmentsToMatch = commitments.Where(c => c.Ukprn == learner.Ukprn).ToList();

            if (commitmentsToMatch.Any())
            {
                return NextMatchHandler.Match(commitmentsToMatch, learner);
            }

            return DataLockErrorCodes.MismatchingUkprn;
        }
    }
}
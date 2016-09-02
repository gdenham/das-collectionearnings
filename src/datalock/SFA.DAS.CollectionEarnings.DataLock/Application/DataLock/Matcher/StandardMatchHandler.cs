using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class StandardMatchHandler : MatchHandler
    {
        public override string Match(List<Data.Entities.Commitment> commitments, Data.Entities.DasLearner learner)
        {
            var commitmentsToMatch = commitments.Where(c => learner.StdCode.HasValue &&
                                                            c.StandardCode.HasValue &&
                                                            c.StandardCode.Value == learner.StdCode.Value).ToList();

            if (commitmentsToMatch.Any())
            {
                return NextMatchHandler.Match(commitmentsToMatch, learner);
            }

            return DataLockErrorCodes.MismatchingStandard;
        }
    }
}
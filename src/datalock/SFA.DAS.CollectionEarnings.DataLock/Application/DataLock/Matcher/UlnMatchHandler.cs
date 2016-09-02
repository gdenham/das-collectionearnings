using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class UlnMatchHandler : MatchHandler
    {
        public override string Match(List<Data.Entities.Commitment> commitments, Data.Entities.DasLearner learner)
        {
            var commitmentsToMatch = commitments.Where(c => learner.Uln.HasValue && c.Uln == learner.Uln.Value).ToList();

            if (commitmentsToMatch.Any())
            {
                return ExecuteNextHandler(commitmentsToMatch, learner);
            }

            return DataLockErrorCodes.MismatchingUln;
        }
    }
}

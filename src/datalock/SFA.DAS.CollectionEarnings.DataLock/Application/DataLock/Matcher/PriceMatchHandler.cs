using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class PriceMatchHandler : MatchHandler
    {
        public override string Match(List<Data.Entities.Commitment> commitments, Data.Entities.DasLearner learner)
        {
            var commitmentsToMatch = commitments.Where(c => learner.NegotiatedPrice.HasValue &&
                                                            (long) c.AgreedCost == learner.NegotiatedPrice.Value).ToList();

            if (!commitmentsToMatch.Any())
            {
                return DataLockErrorCodes.MismatchingPrice;
            }

            return ExecuteNextHandler(commitmentsToMatch, learner);
        }
    }
}
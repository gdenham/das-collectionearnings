using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class PriceMatchHandler : MatchHandler
    {
        public override MatchResult Match(List<Commitment.Commitment> commitments, Learner.Learner learner)
        {
            var commitmentsToMatch = commitments.Where(c => learner.NegotiatedPrice.HasValue &&
                                                            (long) c.NegotiatedPrice == learner.NegotiatedPrice.Value).ToList();

            if (!commitmentsToMatch.Any())
            {
                return new MatchResult
                {
                    ErrorCode = DataLockErrorCodes.MismatchingPrice
                };
            }

            return ExecuteNextHandler(commitmentsToMatch, learner);
        }
    }
}
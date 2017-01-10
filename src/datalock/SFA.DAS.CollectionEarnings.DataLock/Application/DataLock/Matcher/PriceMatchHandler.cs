using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class PriceMatchHandler : MatchHandler
    {
        public override MatchResult Match(List<Commitment.Commitment> commitments, PriceEpisode.PriceEpisode priceEpisode)
        {
            var commitmentsToMatch = commitments.Where(c => priceEpisode.NegotiatedPrice.HasValue &&
                                                            (long) c.NegotiatedPrice == priceEpisode.NegotiatedPrice.Value).ToList();

            if (!commitmentsToMatch.Any())
            {
                return new MatchResult
                {
                    ErrorCode = DataLockErrorCodes.MismatchingPrice
                };
            }

            return ExecuteNextHandler(commitmentsToMatch, priceEpisode);
        }
    }
}
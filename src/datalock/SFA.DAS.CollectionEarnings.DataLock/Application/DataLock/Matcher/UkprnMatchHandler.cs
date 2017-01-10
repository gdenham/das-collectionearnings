using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class UkprnMatchHandler : MatchHandler
    {
        public override MatchResult Match(List<Commitment.Commitment> commitments, PriceEpisode.PriceEpisode priceEpisode)
        {
            var commitmentsToMatch = commitments.Where(c => c.Ukprn == priceEpisode.Ukprn).ToList();

            if (commitmentsToMatch.Any())
            {
                return ExecuteNextHandler(commitmentsToMatch, priceEpisode);
            }

            return new MatchResult
            {
                ErrorCode = DataLockErrorCodes.MismatchingUkprn
            };
        }
    }
}
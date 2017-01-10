using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class StandardMatchHandler : MatchHandler
    {
        public override MatchResult Match(List<Commitment.Commitment> commitments, PriceEpisode.PriceEpisode priceEpisode)
        {
            if (priceEpisode.StandardCode.HasValue)
            {
                var commitmentsToMatch = commitments.Where(c => c.StandardCode.HasValue &&
                                                                c.StandardCode.Value == priceEpisode.StandardCode.Value).ToList();

                if (commitmentsToMatch.Any())
                {
                    return ExecuteNextHandler(commitmentsToMatch, priceEpisode);
                }

                return new MatchResult
                {
                    ErrorCode = DataLockErrorCodes.MismatchingStandard
                };
            }

            return ExecuteNextHandler(commitments, priceEpisode);
        }
    }
}
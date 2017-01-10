using System.Collections.Generic;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class MultipleMatchHandler : MatchHandler
    {
        public override MatchResult Match(List<Commitment.Commitment> commitments, PriceEpisode.PriceEpisode priceEpisode)
        {
            if (commitments.Count > 1)
            {
                return new MatchResult
                {
                    ErrorCode = DataLockErrorCodes.MultipleMatches
                };
            }

            return ExecuteNextHandler(commitments, priceEpisode);
        }
    }
}
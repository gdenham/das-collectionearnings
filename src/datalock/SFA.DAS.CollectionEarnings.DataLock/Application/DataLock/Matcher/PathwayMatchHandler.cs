using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class PathwayMatchHandler : MatchHandler
    {
        public override MatchResult Match(List<Commitment.Commitment> commitments, PriceEpisode.PriceEpisode priceEpisode)
        {
            if (!priceEpisode.StandardCode.HasValue)
            {
                var commitmentsToMatch = commitments.Where(c => c.PathwayCode.HasValue &&
                                                                priceEpisode.PathwayCode.HasValue &&
                                                                c.PathwayCode.Value == priceEpisode.PathwayCode.Value).ToList();

                if (commitmentsToMatch.Any())
                {
                    return ExecuteNextHandler(commitmentsToMatch, priceEpisode);
                }

                return new MatchResult
                {
                    ErrorCode = DataLockErrorCodes.MismatchingPathway
                };
            }

            return ExecuteNextHandler(commitments, priceEpisode);
        }
    }
}
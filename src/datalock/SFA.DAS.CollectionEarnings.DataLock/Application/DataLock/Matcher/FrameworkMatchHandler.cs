using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class FrameworkMatchHandler : MatchHandler
    {
        public override MatchResult Match(List<Commitment.Commitment> commitments, PriceEpisode.PriceEpisode priceEpisode)
        {
            if (!priceEpisode.StandardCode.HasValue)
            {
                var commitmentsToMatch = commitments.Where(c => c.FrameworkCode.HasValue &&
                                                                priceEpisode.FrameworkCode.HasValue &&
                                                                c.FrameworkCode.Value == priceEpisode.FrameworkCode.Value)
                    .ToList();

                if (commitmentsToMatch.Any())
                {
                    return ExecuteNextHandler(commitmentsToMatch, priceEpisode);
                }

                return new MatchResult
                {
                    ErrorCode = DataLockErrorCodes.MismatchingFramework
                };
            }

            return ExecuteNextHandler(commitments, priceEpisode);
        }
    }
}
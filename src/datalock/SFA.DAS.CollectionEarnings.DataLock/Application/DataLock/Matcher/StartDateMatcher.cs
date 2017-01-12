using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class StartDateMatcher : MatchHandler
    {
        public override MatchResult Match(List<Commitment.Commitment> commitments, PriceEpisode.PriceEpisode priceEpisode)
        {
            var commitmentsToMatch = commitments.Where(c => priceEpisode.StartDate >= c.StartDate
                                                            && priceEpisode.StartDate < c.EndDate).ToList();

            if (!commitmentsToMatch.Any())
            {
                return new MatchResult
                {
                    ErrorCode = DataLockErrorCodes.EarlierStartDate
                };
            }

            return ExecuteNextHandler(commitmentsToMatch, priceEpisode);
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class ProgrammeMatchHandler : MatchHandler
    {
        public override MatchResult Match(List<Commitment.Commitment> commitments, PriceEpisode.PriceEpisode priceEpisode)
        {
            if (!priceEpisode.StandardCode.HasValue)
            {
                var commitmentsToMatch = commitments.Where(c => c.ProgrammeType.HasValue &&
                                                                priceEpisode.ProgrammeType.HasValue &&
                                                                c.ProgrammeType.Value == priceEpisode.ProgrammeType.Value)
                    .ToList();

                if (commitmentsToMatch.Any())
                {
                    return ExecuteNextHandler(commitmentsToMatch, priceEpisode);
                }

                return new MatchResult
                {
                    ErrorCode = DataLockErrorCodes.MismatchingProgramme
                };
            }

            return ExecuteNextHandler(commitments, priceEpisode);
        }
    }
}
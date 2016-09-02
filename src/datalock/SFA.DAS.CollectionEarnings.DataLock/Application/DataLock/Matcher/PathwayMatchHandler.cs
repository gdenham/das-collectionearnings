using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class PathwayMatchHandler : MatchHandler
    {
        public override string Match(List<Data.Entities.Commitment> commitments, Data.Entities.DasLearner learner)
        {
            var commitmentsToMatch = commitments.Where(c => c.PathwayCode.HasValue &&
                                                            learner.PwayCode.HasValue &&
                                                            c.PathwayCode.Value == learner.PwayCode.Value).ToList();

            if (commitmentsToMatch.Any())
            {
                return NextMatchHandler.Match(commitmentsToMatch, learner);
            }

            return DataLockErrorCodes.MismatchingPathway;
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class ProgrammeMatchHandler : MatchHandler
    {
        public override string Match(List<Data.Entities.Commitment> commitments, Data.Entities.DasLearner learner)
        {
            if (!learner.StandardCode.HasValue)
            {
                var commitmentsToMatch = commitments.Where(c => c.ProgrammeType.HasValue &&
                                                                learner.ProgrammeType.HasValue &&
                                                                c.ProgrammeType.Value == learner.ProgrammeType.Value)
                    .ToList();

                if (commitmentsToMatch.Any())
                {
                    return ExecuteNextHandler(commitmentsToMatch, learner);
                }

                return DataLockErrorCodes.MismatchingProgramme;
            }

            return ExecuteNextHandler(commitments, learner);
        }
    }
}
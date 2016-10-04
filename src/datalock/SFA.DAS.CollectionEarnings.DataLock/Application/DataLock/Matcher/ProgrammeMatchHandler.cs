using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class ProgrammeMatchHandler : MatchHandler
    {
        public override MatchResult Match(List<Commitment.Commitment> commitments, Infrastructure.Data.Entities.LearnerEntity learner)
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

                return new MatchResult
                {
                    ErrorCode = DataLockErrorCodes.MismatchingProgramme
                };
            }

            return ExecuteNextHandler(commitments, learner);
        }
    }
}
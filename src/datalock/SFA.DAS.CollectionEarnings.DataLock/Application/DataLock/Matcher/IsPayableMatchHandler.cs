using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class IsPayableMatchHandler : MatchHandler
    {
        public override MatchResult Match(List<Commitment.Commitment> commitments, Learner.Learner learner)
        {
            var commitmentsToMatch = commitments.Where(c => c.Payable).ToList();

            if (!commitmentsToMatch.Any())
            {
                return new MatchResult
                {
                    ErrorCode = DataLockErrorCodes.NotPayable
                };
            }

            return ExecuteNextHandler(commitmentsToMatch, learner);
        }
    }
}
﻿using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class UkprnMatchHandler : MatchHandler
    {
        public override MatchResult Match(List<Commitment.Commitment> commitments, Learner.Learner learner)
        {
            var commitmentsToMatch = commitments.Where(c => c.Ukprn == learner.Ukprn).ToList();

            if (commitmentsToMatch.Any())
            {
                return ExecuteNextHandler(commitmentsToMatch, learner);
            }

            return new MatchResult
            {
                ErrorCode = DataLockErrorCodes.MismatchingUkprn
            };
        }
    }
}
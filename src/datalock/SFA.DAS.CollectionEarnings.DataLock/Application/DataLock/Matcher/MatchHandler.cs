﻿using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public abstract class MatchHandler
    {
        protected MatchHandler NextMatchHandler;
        
        public virtual void SetNextMatchHandler(MatchHandler nextMatchHandler)
        {
            NextMatchHandler = nextMatchHandler;
        }

        public abstract MatchResult Match(List<Commitment.Commitment> commitments, Learner.Learner learner);

        protected MatchResult ExecuteNextHandler(List<Commitment.Commitment> commitments, Learner.Learner learner)
        {
            return NextMatchHandler == null
                ? new MatchResult
                {
                    Commitment = commitments.SingleOrDefault()
                }
                : NextMatchHandler.Match(commitments, learner);
        }
    }
}
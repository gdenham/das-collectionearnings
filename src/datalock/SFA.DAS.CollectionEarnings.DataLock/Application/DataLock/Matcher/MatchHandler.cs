using System.Collections.Generic;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public abstract class MatchHandler
    {
        protected MatchHandler NextMatchHandler;
        
        public virtual void SetNextMatchHandler(MatchHandler nextMatchHandler)
        {
            NextMatchHandler = nextMatchHandler;
        }

        public abstract string Match(List<Data.Entities.Commitment> commitments, Data.Entities.DasLearner learner);

        protected string ExecuteNextHandler(List<Data.Entities.Commitment> commitments, Data.Entities.DasLearner learner)
        {
            return NextMatchHandler?.Match(commitments, learner);
        }
    }
}
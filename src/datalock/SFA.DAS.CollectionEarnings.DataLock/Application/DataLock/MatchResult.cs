using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock
{
    public class MatchResult
    {
        public string ErrorCode { get; set; }
        public CommitmentEntity Commitment { get; set; } 
    }
}
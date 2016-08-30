using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Data.Repositories
{
    public interface ICommitmentRepository
    {
        IEnumerable<Commitment> GetAllCommitments();
    }
}
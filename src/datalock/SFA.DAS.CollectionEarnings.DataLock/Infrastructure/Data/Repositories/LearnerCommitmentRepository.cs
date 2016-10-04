using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Repositories
{
    public class LearnerCommitmentRepository : DcfsRepository, ILearnerCommitmentRepository
    {
        private const string LearnerCommitmentDestination = "DataLock.DasLearnerCommitment";

        public LearnerCommitmentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void AddLearnerCommitments(LearnerCommitmentEntity[] learnerCommitments)
        {
            ExecuteBatch(learnerCommitments, LearnerCommitmentDestination);
        }
    }
}
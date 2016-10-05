using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Repositories
{
    public class CommitmentRepository : DcfsRepository, ICommitmentRepository
    {
        private const string CommitmentSource = "Reference.DasCommitments";
        private const string CommitmentColumns = "CommitmentId," +
                                                 "Uln," +
                                                 "Ukprn," +
                                                 "AccountId," +
                                                 "StartDate," +
                                                 "EndDate," +
                                                 "AgreedCost," +
                                                 "StandardCode," +
                                                 "ProgrammeType," +
                                                 "FrameworkCode," +
                                                 "PathwayCode";
        private const string SelectCommitments = "SELECT " + CommitmentColumns + " FROM " + CommitmentSource;

        public CommitmentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public CommitmentEntity[] GetAllCommitments()
        {
            return Query<CommitmentEntity>(SelectCommitments);
        }
    }
}
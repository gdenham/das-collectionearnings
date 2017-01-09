using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Repositories
{
    public class CommitmentRepository : DcfsRepository, ICommitmentRepository
    {
        private const string CommitmentSource = "Reference.DasCommitments";
        private const string CommitmentColumns = "CommitmentId," +
                                                 "VersionId," +
                                                 "Uln," +
                                                 "Ukprn," +
                                                 "AccountId," +
                                                 "StartDate," +
                                                 "EndDate," +
                                                 "AgreedCost," +
                                                 "StandardCode," +
                                                 "ProgrammeType," +
                                                 "FrameworkCode," +
                                                 "PathwayCode," +
                                                 "PaymentStatus," +
                                                 "PaymentStatusDescription," +
                                                 "Payable," +
                                                 "Priority," +
                                                 "EffectiveFrom," +
                                                 "EffectiveTo";
        private const string SelectCommitments = "SELECT " + CommitmentColumns + " FROM " + CommitmentSource;
        private const string SelectProviderCommitments = SelectCommitments + " WHERE Ukprn = @Ukprn";

        public CommitmentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public CommitmentEntity[] GetProviderCommitments(long ukprn)
        {
            return Query<CommitmentEntity>(SelectProviderCommitments, new { ukprn });
        }
    }
}
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Repositories
{
    public class LearnerRepository : DcfsRepository, ILearnerRepository
    {
        private const string LearnerSource = "DataLock.vw_DasLearner";
        private const string LearnerColumns = "Ukprn," +
                                              "LearnRefNumber," +
                                              "Uln," +
                                              "NiNumber," +
                                              "AimSeqNumber," +
                                              "StandardCode," +
                                              "ProgrammeType," +
                                              "FrameworkCode," +
                                              "PathwayCode," +
                                              "LearnStartDate," +
                                              "NegotiatedPrice, " +
                                              "PriceEpisodeIdentifier";
        private const string SelectLearners = "SELECT " + LearnerColumns + " FROM " + LearnerSource;
        private const string SelectProviderLearners = SelectLearners + " WHERE Ukprn = @Ukprn";

        public LearnerRepository(string connectionString)
            : base(connectionString)
        {
        }

        public LearnerEntity[] GetProviderLearners(long ukprn)
        {
            return Query<LearnerEntity>(SelectProviderLearners, new { ukprn });
        }
    }
}
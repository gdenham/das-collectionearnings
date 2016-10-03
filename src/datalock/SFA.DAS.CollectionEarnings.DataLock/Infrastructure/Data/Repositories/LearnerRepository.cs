using System.Collections.Generic;
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
                                              "NegotiatedPrice";
        private const string SelectLearners = "SELECT " + LearnerColumns + " FROM " + LearnerSource;

        public LearnerRepository(string connectionString)
            : base(connectionString)
        {
        }

        public LearnerEntity[] GetAllLearners()
        {
            return Query<LearnerEntity>(SelectLearners);
        }
    }
}
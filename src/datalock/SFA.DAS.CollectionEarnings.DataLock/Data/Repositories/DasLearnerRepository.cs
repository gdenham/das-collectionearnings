using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Data.Repositories
{
    public class DasLearnerRepository : IDasLearnerRepository
    {
        private readonly string _connectionString;

        public DasLearnerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<DasLearner> GetAllDasLearners()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<DasLearner>(DasLearner.SelectAll);
            }
        }
    }
}
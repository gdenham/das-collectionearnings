using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Data.Repositories
{
    public class CommitmentRepository : ICommitmentRepository
    {
        private readonly string _connectionString;

        public CommitmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Commitment> GetAllCommitments()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Commitment>(Commitment.SelectAll);
            }
        }
    }
}
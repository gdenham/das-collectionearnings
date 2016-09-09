using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using SFA.DAS.CollectionEarnings.Calculator.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.Data.Repositories
{
    public class LearningDeliveryToProcessRepository : ILearningDeliveryToProcessRepository
    {
        private readonly string _connectionString;
        public LearningDeliveryToProcessRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<LearningDeliveryToProcess> GetAllLearningDeliveriesToProcess()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<LearningDeliveryToProcess>(LearningDeliveryToProcess.SelectAll);
            }
        }
    }
}
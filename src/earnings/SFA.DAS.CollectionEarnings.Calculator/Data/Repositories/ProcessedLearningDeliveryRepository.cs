using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using FastMember;
using SFA.DAS.CollectionEarnings.Calculator.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.Data.Repositories
{
    public class ProcessedLearningDeliveryRepository : IProcessedLearningDeliveryRepository
    {
        private readonly string _connectionString;

        public ProcessedLearningDeliveryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddProcessedLearningDeliveries(IEnumerable<ProcessedLearningDelivery> deliveries)
        {
            var columns = typeof(ProcessedLearningDelivery).GetProperties().Select(p => p.Name).ToArray();

            using (var bcp = new SqlBulkCopy(_connectionString))
            {
                foreach (var column in columns)
                {
                    bcp.ColumnMappings.Add(column, column);
                }

                bcp.BulkCopyTimeout = 0;
                bcp.DestinationTableName = "Rulebase.AE_LearningDelivery";
                bcp.BatchSize = 1000;

                using (var reader = ObjectReader.Create(deliveries, columns))
                {
                    bcp.WriteToServer(reader);
                }
            }
        }
    }
}
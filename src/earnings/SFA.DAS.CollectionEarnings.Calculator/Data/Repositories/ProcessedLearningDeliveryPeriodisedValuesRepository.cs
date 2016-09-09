using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using FastMember;
using SFA.DAS.CollectionEarnings.Calculator.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.Data.Repositories
{
    public class ProcessedLearningDeliveryPeriodisedValuesRepository : IProcessedLearningDeliveryPeriodisedValuesRepository
    {
        private readonly string _connectionString;

        public ProcessedLearningDeliveryPeriodisedValuesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddProcessedLearningDeliveryPeriodisedValues(IEnumerable<ProcessedLearningDeliveryPeriodisedValues> periodisedValues)
        {
            var columns = typeof(ProcessedLearningDeliveryPeriodisedValues).GetProperties().Select(p => p.Name).ToArray();

            using (var bcp = new SqlBulkCopy(_connectionString))
            {
                foreach (var column in columns)
                {
                    bcp.ColumnMappings.Add(column, column);
                }

                bcp.BulkCopyTimeout = 0;
                bcp.DestinationTableName = "Rulebase.AE_LearningDelivery_PeriodisedValues";
                bcp.BatchSize = 1000;

                using (var reader = ObjectReader.Create(periodisedValues, columns))
                {
                    bcp.WriteToServer(reader);
                }
            }
        }
    }
}
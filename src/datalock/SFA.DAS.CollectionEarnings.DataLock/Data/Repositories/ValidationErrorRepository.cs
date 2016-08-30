using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper.Contrib.Extensions;
using FastMember;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Data.Repositories
{
    public class ValidationErrorRepository : IValidationErrorRepository
    {
        private readonly string _connectionString;

        public ValidationErrorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddValidationError(ValidationError validationError)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                try
                {
                    connection.Insert(validationError);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void AddValidationErrors(IEnumerable<ValidationError> validationErrors)
        {
            var columns = typeof(ValidationError).GetProperties().Select(p => p.Name).ToArray();

            using (var bcp = new SqlBulkCopy(_connectionString))
            {
                foreach (var column in columns)
                {
                    bcp.ColumnMappings.Add(column, column);
                }

                bcp.BulkCopyTimeout = 0;
                bcp.DestinationTableName = "DataLock.ValidationError";
                bcp.BatchSize = 1000;

                using (var reader = ObjectReader.Create(validationErrors, columns))
                {
                    bcp.WriteToServer(reader);
                }
            }
        }
    }
}

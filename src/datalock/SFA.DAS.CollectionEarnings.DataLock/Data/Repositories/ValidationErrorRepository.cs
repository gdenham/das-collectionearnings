using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;
using Dapper.Contrib.Extensions;

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

        public void AddValidationErrors(List<ValidationError> validationErrors)
        {
            throw new NotImplementedException();
        }
    }
}

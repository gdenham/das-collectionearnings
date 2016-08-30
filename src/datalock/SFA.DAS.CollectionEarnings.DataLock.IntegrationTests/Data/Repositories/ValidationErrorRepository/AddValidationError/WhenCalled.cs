using System.Data.SqlClient;
using System.Linq;
using Dapper;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Data.Repositories;
using SFA.DAS.CollectionEarnings.DataLock.Common.Tests.Data;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Data.Repositories.ValidationErrorRepository.AddValidationError
{
    public class WhenCalled
    {
        private readonly string _transientConnectionString = ConnectionStringFactory.GetTransientConnectionString();

        private IValidationErrorRepository _validationErrorRepository;

        [SetUp]
        public void Arrange()
        {
            Database.Clean(_transientConnectionString);

            _validationErrorRepository = new DataLock.Data.Repositories.ValidationErrorRepository(_transientConnectionString);
        }

        [Test]
        public void ThenValidationErrorAddedSuccessfully()
        {
            // Arrange
            var validationError = new ValidationError()
            {
                LearnRefNumber = "Lrn001",
                AimSeqNumber = 1,
                RuleId = "DLOCK_01"
            };

            // Act
            _validationErrorRepository.AddValidationError(validationError);

            // Assert
            using (var connection = new SqlConnection(_transientConnectionString))
            {
                var errors = connection.Query(ValidationError.SelectAll);

                Assert.IsNotNull(errors);
                Assert.AreEqual(1, errors.ToList().Count);
            }
        }
    }
}

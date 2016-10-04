using System.Data.SqlClient;
using System.Linq;
using Dapper;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.ValidationErrorRepository
{
    public class WhenAddValidationErrorCalled
    {
        private IValidationErrorRepository _validationErrorRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _validationErrorRepository = new DataLock.Infrastructure.Data.Repositories.ValidationErrorRepository(GlobalTestContext.Instance.ConnectionString);
        }

        [Test]
        public void ThenValidationErrorAddedSuccessfully()
        {
            // Arrange
            var validationError = new ValidationErrorBuilder().Build();

            // Act
            _validationErrorRepository.AddValidationError(validationError);

            // Assert
            using (var connection = new SqlConnection(GlobalTestContext.Instance.ConnectionString))
            {
                var errors = connection.Query(ValidationErrorEntity.SelectAll);

                Assert.IsNotNull(errors);
                Assert.AreEqual(1, errors.ToList().Count);
            }
        }
    }
}

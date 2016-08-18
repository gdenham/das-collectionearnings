using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.Data.Repositories;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Data.Repositories.ValidationErrorRepository.AddValidationError
{
    public class WhenCalled
    {
        private readonly string _connectionString = "Server=(local); User Id=sa;Password=Password1; Initial Catalog=IlrTransient;";

        private IValidationErrorRepository _validationErrorRepository;

        [SetUp]
        public void Arrange()
        {
            _validationErrorRepository = new DataLock.Data.Repositories.ValidationErrorRepository(_connectionString);
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
            // TODO
        }
    }
}

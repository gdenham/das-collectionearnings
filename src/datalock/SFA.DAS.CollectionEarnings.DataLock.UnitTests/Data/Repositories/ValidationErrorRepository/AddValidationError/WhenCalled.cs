using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.Data.Repositories;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Data.Repositories.ValidationErrorRepository.AddValidationError
{
    public class WhenCalled
    {
        private Mock<IValidationErrorRepository> _validationErrorRepository;

        [SetUp]
        public void Arrange()
        {
            _validationErrorRepository = new Mock<IValidationErrorRepository>();
        }

        [Test]
        public void ThenValidationErrorCalledSuccessfully()
        {
            // Arrange
            var validationError = new ValidationError()
            {
                LearnRefNumber = "Lrn001",
                AimSeqNumber = 1,
                RuleId = "DLOCK_01"
            };

            // Act
            _validationErrorRepository.Object.AddValidationError(validationError);

            // Assert
            _validationErrorRepository.Verify(ver => ver.AddValidationError(validationError), Times.Once());
        }
    }
}

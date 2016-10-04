using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.ValidationError.AddValidationErrorsCommand.AddValidationErrorsCommandHandler
{
    public class WhenHandling
    {
        private Mock<IValidationErrorRepository> _validationErrorRepository;

        private AddValidationErrorsCommandRequest _request;
        private CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand.AddValidationErrorsCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _validationErrorRepository = new Mock<IValidationErrorRepository>();

            _request = new AddValidationErrorsCommandRequest
            {
                ValidationErrors = new[]
                {
                    new ValidationErrorBuilder().Build(),
                    new ValidationErrorBuilder().WithRuleId(DataLockErrorCodes.MismatchingUln).Build()
                }
            };

            _handler = new CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand.AddValidationErrorsCommandHandler(_validationErrorRepository.Object);
        }

        [Test]
        public void ThenSuccessfullForValidRepositoryResponse()
        {
            // Arrange
            _validationErrorRepository
                .Setup(ver => ver.AddValidationErrors(It.IsAny<ValidationErrorEntity[]>()));

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
        }

        [Test]
        public void ThenExceptionIsThrownForInvalidRepositoryResponse()
        {
            // Arrange
            _validationErrorRepository
                .Setup(ver => ver.AddValidationErrors(It.IsAny<ValidationErrorEntity[]>()))
                .Throws(new Exception("Exception while writing validation errors."));

            // Assert
            Assert.Throws<Exception>(() => _handler.Handle(_request));
        }
    }
}
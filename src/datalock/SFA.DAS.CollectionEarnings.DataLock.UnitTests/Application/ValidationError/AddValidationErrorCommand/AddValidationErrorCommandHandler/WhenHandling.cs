using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorCommand;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.ValidationError.AddValidationErrorCommand.AddValidationErrorCommandHandler
{
    public class WhenHandling
    {
        private Mock<IValidationErrorRepository> _validationErrorRepository;

        private AddValidationErrorCommandRequest _request;
        private CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorCommand.AddValidationErrorCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _validationErrorRepository = new Mock<IValidationErrorRepository>();

            _request = new AddValidationErrorCommandRequest
            {
                ValidationError = new ValidationErrorBuilder().Build()
            };

            _handler = new CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorCommand.AddValidationErrorCommandHandler(_validationErrorRepository.Object);
        }

        [Test]
        public void ThenValidResponseReturnedForValidRepositoryResponse()
        {
            // Arrange
            _validationErrorRepository
                .Setup(ver => ver.AddValidationError(It.IsAny<Infrastructure.Data.Entities.ValidationErrorEntity>()));

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.IsNull(response.Exception);
        }

        [Test]
        public void ThenInvalidResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _validationErrorRepository
                .Setup(ver => ver.AddValidationError(It.IsAny<Infrastructure.Data.Entities.ValidationErrorEntity>()))
                .Throws(new Exception("Exception while writing validation error."));

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsValid);
            Assert.IsNotNull(response.Exception);
        }
    }
}
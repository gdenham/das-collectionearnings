﻿using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand;
using SFA.DAS.CollectionEarnings.DataLock.Data.Repositories;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.ValidationError.AddValidationErrorsCommand.AddValidationErrorsCommandHandler.Handle
{
    public class WhenCalled
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
        public void ThenValidResponseReturnedForValidRepositoryResponse()
        {
            // Arrange
            _validationErrorRepository
                .Setup(ver => ver.AddValidationErrors(It.IsAny<IEnumerable<Data.Entities.ValidationError>>()));

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
                .Setup(ver => ver.AddValidationErrors(It.IsAny<IEnumerable<Data.Entities.ValidationError>>()))
                .Throws(new Exception("Exception while writing validation errors."));

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsValid);
            Assert.IsNotNull(response.Exception);
        }
    }
}
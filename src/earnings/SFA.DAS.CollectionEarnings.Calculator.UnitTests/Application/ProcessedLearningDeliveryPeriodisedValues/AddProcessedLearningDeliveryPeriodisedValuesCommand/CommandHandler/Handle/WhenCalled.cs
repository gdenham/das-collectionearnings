﻿using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDeliveryPeriodisedValues.AddProcessedLearningDeliveryPeriodisedValuesCommand;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Application.ProcessedLearningDeliveryPeriodisedValues.AddProcessedLearningDeliveryPeriodisedValuesCommand.CommandHandler.Handle
{
    public class WhenCalled
    {
        private Mock<IProcessedLearningDeliveryPeriodisedValuesRepository> _repository;

        private AddProcessedLearningDeliveryPeriodisedValuesCommandRequest _request;
        private AddProcessedLearningDeliveryPeriodisedValuesCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _repository = new Mock<IProcessedLearningDeliveryPeriodisedValuesRepository>();

            _request = new AddProcessedLearningDeliveryPeriodisedValuesCommandRequest
            {
                PeriodisedValues = new []
                {
                    new ProcessedLearningDeliveryPeriodisedValuesBuilder().Build(),
                    new ProcessedLearningDeliveryPeriodisedValuesBuilder().WithAimSeqNumber(2).Build(),
                    new ProcessedLearningDeliveryPeriodisedValuesBuilder().WithAimSeqNumber(3).Build(),
                    new ProcessedLearningDeliveryPeriodisedValuesBuilder().WithAimSeqNumber(4).Build(),
                    new ProcessedLearningDeliveryPeriodisedValuesBuilder().WithAimSeqNumber(5).Build()
                }
            };

            _handler = new AddProcessedLearningDeliveryPeriodisedValuesCommandHandler(_repository.Object);
        }

        [Test]
        public void ThenValidResponseReturnedForValidRepositoryResponse()
        {
            // Arrange
            _repository
                .Setup(r => r.AddProcessedLearningDeliveryPeriodisedValues(It.IsAny<Infrastructure.Data.Entities.ProcessedLearningDeliveryPeriodisedValues[]>()));

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
        }

        [Test]
        public void ThenInvalidResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _repository
                .Setup(r => r.AddProcessedLearningDeliveryPeriodisedValues(It.IsAny<Infrastructure.Data.Entities.ProcessedLearningDeliveryPeriodisedValues[]>()))
                .Throws<Exception>();

            // Assert
            Assert.Throws<Exception>(() => _handler.Handle(_request));
        }
    }
}
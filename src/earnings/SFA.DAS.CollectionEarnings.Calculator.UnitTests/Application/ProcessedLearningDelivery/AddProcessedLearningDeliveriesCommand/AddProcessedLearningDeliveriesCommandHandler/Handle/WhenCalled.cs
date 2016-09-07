using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDelivery.AddProcessedLearningDeliveriesCommand;
using SFA.DAS.CollectionEarnings.Calculator.Data.Repositories;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Application.ProcessedLearningDelivery.AddProcessedLearningDeliveriesCommand.AddProcessedLearningDeliveriesCommandHandler.Handle
{
    public class WhenCalled
    {
        private Mock<IProcessedLearningDeliveryRepository> _repository;

        private AddProcessedLearningDeliveriesCommandRequest _request;
        private Calculator.Application.ProcessedLearningDelivery.AddProcessedLearningDeliveriesCommand.AddProcessedLearningDeliveriesCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _repository = new Mock<IProcessedLearningDeliveryRepository>();

            _request = new AddProcessedLearningDeliveriesCommandRequest
            {
                LearningDeliveries = new []
                {
                    new ProcessedLearningDeliveryBuilder().Build(),
                    new ProcessedLearningDeliveryBuilder().WithAimSeqNumber(2).Build(),
                    new ProcessedLearningDeliveryBuilder().WithAimSeqNumber(3).Build(),
                    new ProcessedLearningDeliveryBuilder().WithAimSeqNumber(4).Build(),
                    new ProcessedLearningDeliveryBuilder().WithAimSeqNumber(5).Build()
                }
            };

            _handler = new Calculator.Application.ProcessedLearningDelivery.AddProcessedLearningDeliveriesCommand.AddProcessedLearningDeliveriesCommandHandler(_repository.Object);
        }

        [Test]
        public void ThenValidResponseReturnedForValidRepositoryResponse()
        {
            // Arrange
            _repository
                .Setup(r => r.AddProcessedLearningDeliveries(It.IsAny<IEnumerable<Data.Entities.ProcessedLearningDelivery>>()));

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
            _repository
                .Setup(r => r.AddProcessedLearningDeliveries(It.IsAny<IEnumerable<Data.Entities.ProcessedLearningDelivery>>()))
                .Throws(new Exception("Error while writing processed learning deliveries."));

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsValid);
            Assert.IsNotNull(response.Exception);
        }
    }
}
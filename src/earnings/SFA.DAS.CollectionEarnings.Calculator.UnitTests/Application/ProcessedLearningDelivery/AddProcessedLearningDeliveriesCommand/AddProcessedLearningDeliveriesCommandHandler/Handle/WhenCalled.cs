using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDelivery.AddProcessedLearningDeliveriesCommand;
using SFA.DAS.CollectionEarnings.Calculator.Data;
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
                .Setup(r => r.AddProcessedLearningDeliveries(It.IsAny<Data.Entities.ProcessedLearningDelivery[]>()));

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
                .Setup(r => r.AddProcessedLearningDeliveries(It.IsAny<Data.Entities.ProcessedLearningDelivery[]>()))
                .Throws<Exception>();

            // Assert
            Assert.Throws<Exception>(() => _handler.Handle(_request));
        }
    }
}
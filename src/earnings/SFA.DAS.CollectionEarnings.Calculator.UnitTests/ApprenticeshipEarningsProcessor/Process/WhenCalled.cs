using System;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery;
using SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery;
using SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDelivery.AddProcessedLearningDeliveriesCommand;
using SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDeliveryPeriod.AddProcessedLearningDeliveryPeriodCommand;
using SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDeliveryPeriodisedValues.AddProcessedLearningDeliveryPeriodisedValuesCommand;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.ApprenticeshipEarningsProcessor.Process
{
    public class WhenCalled
    {
        private static readonly object[] EmptyItems =
        {
            new object[] {null},
            new object[] {new LearningDeliveryToProcess[] {}}
        };

        private Calculator.ApprenticeshipEarningsProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new Calculator.ApprenticeshipEarningsProcessor(_logger.Object, _mediator.Object);

            InitialMockSetup();
        }

        private void InitialMockSetup()
        {
            // Mediator
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllLearningDeliveriesToProcessQueryRequest>()))
                .Returns(new GetAllLearningDeliveriesToProcessQueryResponse
                    {
                        IsValid = true,
                        Items = new[]
                            {
                                new LearningDeliveryToProcessBuilder().Build()
                            }
                    }
                );

            _mediator
                .Setup(m => m.Send(It.IsAny<GetLearningDeliveriesEarningsQueryRequest>()))
                .Returns(new GetLearningDeliveriesEarningsQueryResponse
                    {
                        IsValid = true,
                        ProcessedLearningDeliveries = new[] { new ProcessedLearningDeliveryBuilder().Build() },
                        ProcessedLearningDeliveryPeriodisedValues = new[] { new ProcessedLearningDeliveryPeriodisedValuesBuilder().Build() }
                    }
                );

            _mediator
                .Setup(m => m.Send(It.IsAny<AddProcessedLearningDeliveriesCommandRequest>()))
                .Returns(Unit.Value);

            _mediator
                .Setup(m => m.Send(It.IsAny<AddProcessedLearningDeliveryPeriodisedValuesCommandRequest>()))
                .Returns(Unit.Value);

            _mediator
                .Setup(m => m.Send(It.IsAny<AddProcessedLearningDeliveryPeriodCommandRequest>()))
                .Returns(Unit.Value);
        }

        [Test]
        public void ThenExpectingExceptionForGetAllLearningDeliveriesToProcessQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllLearningDeliveriesToProcessQueryRequest>()))
                .Returns(new GetAllLearningDeliveriesToProcessQueryResponse
                    {
                        IsValid = false,
                        Exception = new Exception(EarningsCalculatorException.ErrorReadingLearningDeliveriesToProcessMessage)
                    }
                );

            // Assert
            var ex = Assert.Throws<EarningsCalculatorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorException.ErrorReadingLearningDeliveriesToProcessMessage));
        }

        [Test]
        public void ThenExpectingExceptionForGetLearningDeliveriesEarningsQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetLearningDeliveriesEarningsQueryRequest>()))
                .Returns(new GetLearningDeliveriesEarningsQueryResponse
                    {
                        IsValid = false,
                        Exception = new Exception(EarningsCalculatorException.ErrorCalculatingEarningsForTheLearningDeliveriesMessage)
                    }
                );

            // Assert
            var ex = Assert.Throws<EarningsCalculatorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorException.ErrorCalculatingEarningsForTheLearningDeliveriesMessage));
        }

        [Test]
        public void ThenExpectingExceptionForAddProcessedLearningDeliveriesCommandFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<AddProcessedLearningDeliveriesCommandRequest>()))
                .Throws<Exception>();

            // Assert
            var ex = Assert.Throws<EarningsCalculatorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorException.ErrorWritingProcessedLearningDeliveriesMessage));
        }

        [Test]
        public void ThenExpectingExceptionForAddProcessedLearningDeliveryPeriodisedValuesCommandFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<AddProcessedLearningDeliveryPeriodisedValuesCommandRequest>()))
                .Throws<Exception>();

            // Assert
            var ex = Assert.Throws<EarningsCalculatorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorException.ErrorWritingProcessedLearningDeliveryPeriodisedValuesMessage));
        }

        [Test]

        [TestCaseSource(nameof(EmptyItems))]
        public void ThenExpectingLogEntryForGetAllLearningDeliveriesToProcessQueryNoItems(LearningDeliveryToProcess[] items)
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllLearningDeliveriesToProcessQueryRequest>()))
                .Returns(new GetAllLearningDeliveriesToProcessQueryResponse
                    {
                        IsValid = true,
                        Items = items
                    }
                );

            // Act
            _processor.Process();

            // Assert
            _logger.Verify(l => l.Info(It.Is<string>(p => p.Equals("Not found any learning deliveries to process."))), Times.Once);
        }

        [Test]
        public void ThenExpectingExceptionForAddProcessedLearningDeliveryPeriodCommandFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<AddProcessedLearningDeliveryPeriodCommandRequest>()))
                .Throws<Exception>();

            // Assert
            var ex = Assert.Throws<EarningsCalculatorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorException.ErrorWritingLearningDeliveriesPeriodEarningsMessage));
        }
    }
}
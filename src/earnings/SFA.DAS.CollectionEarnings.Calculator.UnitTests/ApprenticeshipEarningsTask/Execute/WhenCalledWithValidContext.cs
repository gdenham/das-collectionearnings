using System;
using System.Collections.Generic;
using CS.Common.External.Interfaces;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Context;
using SFA.DAS.CollectionEarnings.Calculator.DependencyResolution;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools;
using NLog;
using SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery;
using SFA.DAS.CollectionEarnings.Calculator.Data.Entities;
using SFA.DAS.CollectionEarnings.Calculator.Exceptions;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.ApprenticeshipEarningsTask.Execute
{
    public class WhenCalledWithValidContext
    {
        private static readonly object[] EmptyItems =
        {
            new object[] {null},
            new object[] {new LearningDeliveryToProcess[] {}}
        };

        private IExternalContext _context;
        private IExternalTask _task;

        private Mock<IDependencyResolver> _dependencyResolver;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;
        private Mock<ApprenticeshipEarningsProcessor> _processor;

        [SetUp]
        public void Arrange()
        {
            _context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String"},
                    {ContextPropertyKeys.LogLevel, "Info"}
                }
            };

            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();
            _processor = new Mock<ApprenticeshipEarningsProcessor>();
            _dependencyResolver = new Mock<IDependencyResolver>();

            InitialMockSetup();

            _task = new Calculator.ApprenticeshipEarningsTask(_dependencyResolver.Object, _logger.Object);
        }

        private void InitialMockSetup()
        {
            // Dependency Resolver
            _dependencyResolver
                .Setup(dr => dr.GetInstance<ILogger>())
                .Returns(_logger.Object);
            _dependencyResolver
                .Setup(dr => dr.GetInstance<IMediator>())
                .Returns(_mediator.Object);
            _dependencyResolver
                .Setup(dr => dr.GetInstance<ApprenticeshipEarningsProcessor>())
                .Returns(_processor.Object);

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
                        Exception = new Exception("Error while reading learning deliveries to process.")
                    }
                );

            _dependencyResolver
                .Setup(dr => dr.GetInstance<ApprenticeshipEarningsProcessor>())
                .Returns(new ApprenticeshipEarningsProcessor(_logger.Object, _mediator.Object));

            // Assert
            var ex = Assert.Throws<EarningsCalculatorProcessorException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains("Error while reading learning deliveries to process."));
        }

        [Test]

        [TestCaseSource(nameof(EmptyItems))]
        public void ThenExpectingLogEntryForGetAllLearningDeliveriesToProcessQueryNoItems(IEnumerable<LearningDeliveryToProcess> items)
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

            _dependencyResolver
                .Setup(dr => dr.GetInstance<ApprenticeshipEarningsProcessor>())
                .Returns(new ApprenticeshipEarningsProcessor(_logger.Object, _mediator.Object));

            // Act
            _task.Execute(_context);

            // Assert
            _logger.Verify(l => l.Info(It.Is<string>(p => p.Equals("Not found any learning deliveries to process."))), Times.Once);
        }

        [Test]
        public void ThenProcessorIsExecuted()
        {
            // Arrange
            _dependencyResolver
                .Setup(dr => dr.GetInstance<ApprenticeshipEarningsProcessor>())
                .Returns(_processor.Object);

            // Act
            var properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" },
                { ContextPropertyKeys.LogLevel, "Info" }
            };

            _context.Properties = properties;

            _task.Execute(_context);

            // Assert
            _processor.Verify(p => p.Process(), Times.Once);
        }
    }
}
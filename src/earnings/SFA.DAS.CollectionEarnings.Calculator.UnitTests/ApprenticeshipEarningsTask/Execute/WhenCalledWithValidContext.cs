using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Context;
using SFA.DAS.CollectionEarnings.Calculator.DependencyResolution;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools;
using NLog;
using SFA.DAS.CollectionEarnings.Calculator.Data.Entities;

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
        private Mock<Calculator.ApprenticeshipEarningsProcessor> _processor;

        [SetUp]
        public void Arrange()
        {
            _context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String"},
                    {ContextPropertyKeys.LogLevel, "Info"},
                    {ContextPropertyKeys.YearOfCollection, "1718"}
                }
            };

            _dependencyResolver = new Mock<IDependencyResolver>();
            _logger = new Mock<ILogger>();
            _processor = new Mock<Calculator.ApprenticeshipEarningsProcessor>();

            _task = new Calculator.ApprenticeshipEarningsTask(_dependencyResolver.Object, _logger.Object);
        }

        [Test]
        public void ThenProcessorIsExecuted()
        {
            // Arrange
            _dependencyResolver
                .Setup(dr => dr.GetInstance<Calculator.ApprenticeshipEarningsProcessor>())
                .Returns(_processor.Object);

            // Act
            _task.Execute(_context);

            // Assert
            _processor.Verify(p => p.Process(), Times.Once);
        }
    }
}
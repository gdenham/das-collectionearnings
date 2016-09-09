using System.Collections.Generic;
using CS.Common.External.Interfaces;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Context;
using SFA.DAS.CollectionEarnings.Calculator.DependencyResolution;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools;
using NLog;
using SFA.DAS.CollectionEarnings.Calculator.Exceptions;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.ApprenticeshipEarningsTask.Execute
{
    public class WhenCalledWithInvalidContext
    {
        private static readonly object[] EmptyProperties =
        {
            new object[] {null},
            new object[] {new Dictionary<string, string>()}
        };

        private IExternalContext _context = new ExternalContextStub();
        private IExternalTask _task;

        private Mock<IDependencyResolver> _dependencyResolver;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver.Setup(dr => dr.GetInstance<ILogger>()).Returns(_logger.Object);
            _dependencyResolver.Setup(dr => dr.GetInstance<IMediator>()).Returns(_mediator.Object);

            _task = new Calculator.ApprenticeshipEarningsTask(_dependencyResolver.Object, _logger.Object);
        }

        [Test]
        public void ThenExpectingExceptionForNullContextProvided()
        {
            // Assert
            var ex = Assert.Throws<EarningsCalculatorInvalidContextException>(() => _task.Execute(null));
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorExceptionMessages.ContextNull));
        }

        [Test]
        [TestCaseSource(nameof(EmptyProperties))]
        public void ThenExpectingExceptionForNoContextPropertiesProvided(IDictionary<string, string> properties)
        {
            _context.Properties = properties;

            // Assert
            var ex = Assert.Throws<EarningsCalculatorInvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorExceptionMessages.ContextNoProperties));
        }

        [Test]
        public void ThenExpectingExceptionForNoConnectionStringProvided()
        {
            var properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.LogLevel, "Info" },
                { ContextPropertyKeys.YearOfCollection, "1617" }
            };

            _context.Properties = properties;

            // Assert
            var ex = Assert.Throws<EarningsCalculatorInvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorExceptionMessages.ContextPropertiesNoConnectionString));
        }

        [Test]
        public void ThenExpectingExceptionForNoLogLevelProvided()
        {
            var properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" },
                { ContextPropertyKeys.YearOfCollection, "1617" }
            };

            _context.Properties = properties;

            // Assert
            var ex = Assert.Throws<EarningsCalculatorInvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorExceptionMessages.ContextPropertiesNoLogLevel));
        }

        [Test]
        public void ThenExpectingExceptionForNoYearOfCollectionProvided()
        {
            var properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" },
                { ContextPropertyKeys.LogLevel, "Info" }
            };

            _context.Properties = properties;

            // Assert
            var ex = Assert.Throws<EarningsCalculatorInvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorExceptionMessages.ContextPropertiesNoYearOfCollection));
        }

        [TestCase(" ")]
        [TestCase("acbd")]
        [TestCase("12345")]
        [TestCase("1234")]
        [TestCase("9999")]
        public void ThenExpectingExceptionForInvalidYearOfCollectionProvided(string yearOfCollection)
        {
            var properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" },
                { ContextPropertyKeys.LogLevel, "Info" },
                { ContextPropertyKeys.YearOfCollection, yearOfCollection }
            };

            _context.Properties = properties;

            // Assert
            var ex = Assert.Throws<EarningsCalculatorInvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorExceptionMessages.ContextPropertiesInvalidYearOfCollection));
        }
    }
}
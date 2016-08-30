using System.Collections.Generic;
using CS.Common.External.Interfaces;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Common.Tests.ExternalContext;
using SFA.DAS.CollectionEarnings.DataLock.Context;
using SFA.DAS.CollectionEarnings.DataLock.DependencyResolution;
using SFA.DAS.CollectionEarnings.DataLock.Exceptions;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.DataLockTask.Execute
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

            _task = new DataLock.DataLockTask(_dependencyResolver.Object);
        }

        [Test]
        public void ThenExpectingExceptionForNullContextProvided()
        {
            // Assert
            var ex = Assert.Throws<DataLockInvalidContextException>(() => _task.Execute(null));
            Assert.IsTrue(ex.Message.Contains(DataLockExceptionMessages.ContextNull));
        }

        [Test]
        [TestCaseSource(nameof(EmptyProperties))]
        public void ThenExpectingExceptionForNoContextPropertiesProvided(IDictionary<string, string> properties)
        {
            _context.Properties = properties;

            // Assert
            var ex = Assert.Throws<DataLockInvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(DataLockExceptionMessages.ContextNoProperties));
        }

        [Test]
        public void ThenExpectingExceptionForNoConnectionStringProvided()
        {
            var properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.LogLevel, "Info" }
            };

            _context.Properties = properties;

            // Assert
            var ex = Assert.Throws<DataLockInvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(DataLockExceptionMessages.ContextPropertiesNoConnectionString));
        }

        [Test]
        public void ThenExpectingExceptionForNoLogLevelProvided()
        {
            var properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" }
            };

            _context.Properties = properties;

            // Assert
            var ex = Assert.Throws<DataLockInvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(DataLockExceptionMessages.ContextPropertiesNoLogLevel));
        }
    }
}
using CS.Common.External.Interfaces;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Domain.DependencyResolution;
using SFA.DAS.CollectionEarnings.Infrastructure.DcContext;
using System;
using System.Collections.Generic;

/*
 * Unit tests pattern under review. Might be changed in other solutions. Should not be taken as reference.
 */
namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.DataLockTaskTests
{
    public class WhenExecuting
    {
        private IExternalContext _context = new ExternalContext();
        private IExternalTask _task;

        private Mock<IDependencyResolver> _dependencyResolver;
        private Mock<ILogger> _logger;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver.Setup(dr => dr.GetInstance<ILogger>()).Returns(_logger.Object);

            _task = new DataLockTask(_dependencyResolver.Object);
        }

        [Test]
        public void WithNoConnectionStringThenExpectingException()
        {
            var properties = new Dictionary<string, string>
            {
                { DcContextPropertyKeys.LogLevel, "Info" }
            };

            _context.Properties = properties;

            // Assert
            Assert.That(() => _task.Execute(_context), Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void WithNoLogLevelThenExpectingException()
        {
            var properties = new Dictionary<string, string>
            {
                { DcContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" }
            };

            _context.Properties = properties;

            // Assert
            Assert.That(() => _task.Execute(_context), Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void WithVlidContextThenLoggingIsDone()
        {
            // Act
            var properties = new Dictionary<string, string>
            {
                { DcContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" },
                { DcContextPropertyKeys.LogLevel, "Info" }
            };

            _context.Properties = properties;

            _task.Execute(_context);

            // Assert
            _logger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        internal class ExternalContext : IExternalContext
        {
            public IDictionary<string, string> Properties { get; set; }
        }
    }
}

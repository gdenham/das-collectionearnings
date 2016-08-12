using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Contract;
using SFA.DAS.CollectionEarnings.Domain.DependencyResolution;
using System;
using System.Collections.Generic;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.DataLockTaskTests
{
    public class WhenExecuting
    {
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
            var context = new Dictionary<string, string>
            {
                { DasContextPropertyKeys.LogLevel, "Info" }
            };

            // Assert
            Assert.That(() => _task.Execute(context), Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void WithNoLogLevelThenExpectingException()
        {
            var context = new Dictionary<string, string>
            {
                { DasContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" }
            };

            // Assert
            Assert.That(() => _task.Execute(context), Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void WithVlidContextThenLoggingIsDone()
        {
            // Act
            var context = new Dictionary<string, string>
            {
                { DasContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" },
                { DasContextPropertyKeys.LogLevel, "Info" }
            };

            _task.Execute(context);

            // Assert
            _logger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}

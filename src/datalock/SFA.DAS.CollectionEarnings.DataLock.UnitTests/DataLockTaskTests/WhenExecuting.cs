﻿using CS.Common.External.Interfaces;
using Moq;
using NLog;
using NUnit.Framework;
using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.DataLock.Common.Tests.ExternalContext;
using SFA.DAS.CollectionEarnings.DataLock.Context;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.Data.Repositories;
using SFA.DAS.CollectionEarnings.DataLock.DependencyResolution;
using SFA.DAS.CollectionEarnings.DataLock.Exceptions;

/*
 * Unit tests pattern under review. Might be changed in other solutions. Should not be taken as reference.
 */
//TODO Change test format or remove comments
namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.DataLockTaskTests
{
    public class WhenExecuting
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
        private Mock<IValidationErrorRepository> _validationErrorRepository;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _validationErrorRepository = new Mock<IValidationErrorRepository>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver.Setup(dr => dr.GetInstance<ILogger>()).Returns(_logger.Object);
            _dependencyResolver.Setup(dr => dr.GetInstance<IValidationErrorRepository>()).Returns(_validationErrorRepository.Object);

            _task = new DataLockTask(_dependencyResolver.Object);
        }

        [Test]
        public void WithNullContextThenExpectingException()
        {
            // Assert
            var ex = Assert.Throws<DataLockInvalidContextException>(() => _task.Execute(null));
            Assert.IsTrue(ex.Message.Contains(DataLockExceptionMessages.ContextNull));
        }

        [Test]
        [TestCaseSource(nameof(EmptyProperties))]
        public void WithNoContextPropertiesThenExpectingException(IDictionary<string, string> properties)
        {
            _context.Properties = properties;

            // Assert
            var ex = Assert.Throws<DataLockInvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(DataLockExceptionMessages.ContextNoProperties));
        }

        [Test]
        public void WithNoConnectionStringThenExpectingException()
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
        public void WithNoLogLevelThenExpectingException()
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

        [Test]
        public void WithValidContextThenTaskIsExecuted()
        {
            // Act
            var properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" },
                { ContextPropertyKeys.LogLevel, "Info" }
            };

            _context.Properties = properties;

            _task.Execute(_context);

            // Assert
            _logger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(2));
            _validationErrorRepository.Verify(ver => ver.AddValidationError(It.IsAny<ValidationError>()), Times.Once());
        }
    }
}

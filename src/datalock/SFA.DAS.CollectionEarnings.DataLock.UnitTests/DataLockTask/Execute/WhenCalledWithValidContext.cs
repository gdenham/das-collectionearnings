using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Context;
using SFA.DAS.CollectionEarnings.DataLock.DependencyResolution;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.DataLockTask.Execute
{
    public class WhenCalledWithValidContext
    {
        private IExternalContext _context;
        private IExternalTask _task;

        private Mock<IDependencyResolver> _dependencyResolver;
        private Mock<ILogger> _logger;
        private Mock<DataLock.DataLockProcessor> _processor;

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

            _dependencyResolver = new Mock<IDependencyResolver>();
            _logger = new Mock<ILogger>();
            _processor = new Mock<DataLock.DataLockProcessor>();

            _task = new DataLock.DataLockTask(_dependencyResolver.Object, _logger.Object);
        }

        [Test]
        public void ThenProcessorIsExecuted()
        {
            // Arrange
            _dependencyResolver
                .Setup(dr => dr.GetInstance<DataLock.DataLockProcessor>())
                .Returns(_processor.Object);

            // Act
            _task.Execute(_context);

            // Assert
            _processor.Verify(p => p.Process(), Times.Once);
        }
    }
}
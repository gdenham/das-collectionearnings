using System.Collections.Generic;
using NLog;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Context;
using SFA.DAS.CollectionEarnings.DataLock.Data.Repositories;
using SFA.DAS.CollectionEarnings.DataLock.DependencyResolution;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;

/*
 * Unit tests pattern under review. Might be changed in other solutions. Should not be taken as reference.
 */
//TODO Change test format or remove comments
namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.DependencyResolution.TaskDependencyResolverTests
{
    public class WhenInitialising
    {
        private TaskDependencyResolver _dependencyResolver;

        [SetUp]
        public void Arrange()
        {
            var context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>()
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String"},
                    {ContextPropertyKeys.LogLevel, "Info"}
                }
            };

            _dependencyResolver = new TaskDependencyResolver();
            _dependencyResolver.Init(
                typeof(WhenInitialising),
                new ContextWrapper(context)
            );
        }

        [Test]
        public void ThenTheLoggerCanBeResolvedForThatType()
        {
            // Act
            var logger = _dependencyResolver.GetInstance<ILogger>();

            // Assert
            Assert.IsNotNull(logger);
        }

        [Test]
        public void ThenTheLoggerHasBeenNamedWithTheFullNameOfThatType()
        {
            // Act
            var logger = _dependencyResolver.GetInstance<ILogger>();

            // Assert
            Assert.AreEqual(typeof(WhenInitialising).FullName, logger.Name);
        }

        [Test]
        public void ThenTheValidationErrorRepositoryCanBeResolved()
        {
            // Act
            var validationErrorRepositoy = _dependencyResolver.GetInstance<IValidationErrorRepository>();

            // Assert
            Assert.IsNotNull(validationErrorRepositoy);
        }
    }
}

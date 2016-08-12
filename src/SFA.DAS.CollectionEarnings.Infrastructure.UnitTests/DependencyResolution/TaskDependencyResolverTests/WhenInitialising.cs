using NLog;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Infrastructure.DependencyResolution;

namespace SFA.DAS.CollectionEarnings.Infrastructure.UnitTests.DependencyResolution.TaskDependencyResolverTests
{
    public class WhenInitialising
    {
        private TaskDependencyResolver _dependencyResolver;

        [SetUp]
        public void Arrange()
        {
            _dependencyResolver = new TaskDependencyResolver();
            _dependencyResolver.Init(typeof(WhenInitialising));
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
    }
}

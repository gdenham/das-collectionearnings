using NLog;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.DependencyResolution;

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

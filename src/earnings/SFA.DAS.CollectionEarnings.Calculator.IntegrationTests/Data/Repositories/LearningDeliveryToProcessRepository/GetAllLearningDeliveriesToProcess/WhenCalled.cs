using System.Linq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Data;
using SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Tools;

namespace SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Data.Repositories.LearningDeliveryToProcessRepository.GetAllLearningDeliveriesToProcess
{
    public class WhenCalled
    {
        private readonly string _transientConnectionString = ConnectionStringFactory.GetTransientConnectionString();

        private ILearningDeliveryToProcessRepository _repository;

        [SetUp]
        public void Arrange()
        {
            Database.Clean(_transientConnectionString);

            _repository = new Calculator.Data.Repositories.LearningDeliveryToProcessRepository(_transientConnectionString);
        }

        [Test]
        public void ThenNoLearningDeliveriesReturnedForNoEntriesInTheDatabase()
        {
            // Act
            var learningDeliveries = _repository.GetAllLearningDeliveriesToProcess();

            // Assert
            Assert.IsNotNull(learningDeliveries);
            Assert.AreEqual(0, learningDeliveries.Count());
        }

        [Test]
        public void ThenNoLearningDeliveriesReturnedForWrongEntriesInTheDatabase()
        {
            // Arrange
            Database.AddIlrDataNoLearningDeliveriesToProcess(_transientConnectionString);

            // Act
            var learningDeliveries = _repository.GetAllLearningDeliveriesToProcess();

            // Assert
            Assert.IsNotNull(learningDeliveries);
            Assert.AreEqual(0, learningDeliveries.Count());
        }

        [Test]
        public void ThenOneLearningDeliveryReturnedForOneEntryInTheDatabase()
        {
            // Arrange
            Database.AddIlrDataOneLearningDeliveryToProcess(_transientConnectionString);

            // Act
            var learningDeliveries = _repository.GetAllLearningDeliveriesToProcess();

            // Assert
            Assert.IsNotNull(learningDeliveries);
            Assert.AreEqual(1, learningDeliveries.Count());
        }

        [Test]
        public void ThenMultipleLearningDeliveriesReturnedForMultipleEntriesInTheDatabase()
        {
            // Arrange
            Database.AddIlrDataMultipleLearningDeliveriesToProcess(_transientConnectionString);

            // Act
            var learningDeliveries = _repository.GetAllLearningDeliveriesToProcess();

            // Assert
            Assert.IsNotNull(learningDeliveries);
            Assert.AreEqual(10, learningDeliveries.Count());
        }
    }
}
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.LearnerRepository
{
    public class WhenGetProviderLearnersCalledDuringAnIlrPeriodEnd
    {
        private readonly long _ukprn = 10007459;
        private readonly long _ukprnNoLearners = 10007458;

        private ILearnerRepository _learnerRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.PeriodEndClean();

            _learnerRepository = new DataLock.Infrastructure.Data.Repositories.LearnerRepository(GlobalTestContext.Instance.PeriodEndConnectionString);
        }

        [Test]
        public void ThenNoLearnersReturnedForNoEntriesInTheDatabase()
        {
            // Act
            var learners = _learnerRepository.GetProviderLearners(_ukprn);

            // Assert
            Assert.IsNotNull(learners);
            Assert.AreEqual(0, learners.Length);
        }

        [Test]
        public void ThenNoLearnersReturnedForAUkprnWithNoEntriesInTheDatabase()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndMultipleLearningDeliveries.sql");

            // Act
            var learners = _learnerRepository.GetProviderLearners(_ukprnNoLearners);

            // Assert
            Assert.IsNotNull(learners);
            Assert.AreEqual(0, learners.Length);
        }

        [Test]
        public void ThenLearnersReturnedForMultipleEntriesInTheDatabase()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndMultipleLearningDeliveries.sql");

            // Act
            var learners = _learnerRepository.GetProviderLearners(_ukprn);

            // Assert
            Assert.IsNotNull(learners);
            Assert.AreEqual(8, learners.Length);
        }
    }
}
using System;
using System.Linq;
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
            TestDataHelper.Clean();

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
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            var learners = _learnerRepository.GetProviderLearners(_ukprn);

            // Assert
            Assert.IsNotNull(learners);
            Assert.AreEqual(7, learners.Length);
        }

        [Test]
        public void ThenLearnersReturnedForOneLearnerWithMultipleNegotiatedPriceEpisodesInTheDatabase()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndLearnerChangesEmployers.sql");
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            var singleLearnerPriceEpisodes = _learnerRepository.GetProviderLearners(_ukprn);

            // Assert
            Assert.IsNotNull(singleLearnerPriceEpisodes);
            Assert.AreEqual(2, singleLearnerPriceEpisodes.Length);
            Assert.AreEqual(1, singleLearnerPriceEpisodes.Count(l => l.LearnStartDate == new DateTime(2017, 8, 1)));
            Assert.AreEqual(1, singleLearnerPriceEpisodes.Count(l => l.LearnStartDate == new DateTime(2017, 11, 1)));
        }

        [Test]
        public void ThenOneLearnerReturnedForOneLearnerThatChangesEmployersFromDasToNonDasInTheDatabase()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndLearnerChangesEmployersDasToNonDas.sql");
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            var learners = _learnerRepository.GetProviderLearners(_ukprn);

            // Assert
            Assert.IsNotNull(learners);
            Assert.AreEqual(1, learners.Length);
            Assert.AreEqual(1, learners.Count(l => l.LearnStartDate == new DateTime(2017, 8, 1)));
        }
    }
}
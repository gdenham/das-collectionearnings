using System.Linq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools.Ilr;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.LearnerRepository
{
    public class WhenGetProviderLearnersCalled
    {
        private readonly long _ukprn = 10007459;
        private readonly long _ukprnNoLearners = 10007458;

        private ILearnerRepository _learnerRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _learnerRepository = new DataLock.Infrastructure.Data.Repositories.LearnerRepository(GlobalTestContext.Instance.ConnectionString);
        }

        [Test]
        public void ThenNoLearnersReturnedForNoEntriesInTheDatabase()
        {
            // Act
            var dasLearners = _learnerRepository.GetProviderLearners(_ukprn);

            // Assert
            Assert.IsNotNull(dasLearners);
            Assert.AreEqual(0, dasLearners.Count());
        }

        [Test]
        public void ThenNoLearnersReturnedForAUkprnWithNoEntriesInTheDatabase()
        {
            // Arrange
            var shredder = new Shredder();
            shredder.Shred();

            // Act
            var response = _learnerRepository.GetProviderLearners(_ukprnNoLearners);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(0, response.Length);
        }

        [Test]
        public void ThenLearnersReturnedForMultipleEntriesInTheDatabase()
        {
            // Arrange
            var shredder = new Shredder();
            shredder.Shred();

            // Act
            var dasLearners = _learnerRepository.GetProviderLearners(_ukprn);

            // Assert
            Assert.IsNotNull(dasLearners);
            Assert.AreEqual(2, dasLearners.Count());
        }
    }
}
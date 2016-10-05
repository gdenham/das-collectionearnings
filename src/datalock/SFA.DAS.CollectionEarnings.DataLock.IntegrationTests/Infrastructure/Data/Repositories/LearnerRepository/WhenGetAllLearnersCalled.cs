using System.Linq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools.Ilr;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.LearnerRepository
{
    public class WhenGetAllLearnersCalled
    {
        private ILearnerRepository _dasLearnerRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _dasLearnerRepository = new DataLock.Infrastructure.Data.Repositories.LearnerRepository(GlobalTestContext.Instance.ConnectionString);
        }

        [Test]
        public void ThenNoDasLearnersReturnedForNoEntriesInTheDatabase()
        {
            // Act
            var dasLearners = _dasLearnerRepository.GetAllLearners();

            // Assert
            Assert.IsNotNull(dasLearners);
            Assert.AreEqual(0, dasLearners.Count());
        }

        [Test]
        public void ThenDasLearnersReturnedForMultipleEntriesInTheDatabase()
        {
            // Arrange
            var shredder = new Shredder();
            shredder.Shred();

            // Act
            var dasLearners = _dasLearnerRepository.GetAllLearners();

            // Assert
            Assert.IsNotNull(dasLearners);
            Assert.AreEqual(2, dasLearners.Count());
        }
    }
}
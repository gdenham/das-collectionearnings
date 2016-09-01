using System.Linq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Data.Repositories;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools.Ilr;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Data.Repositories.DasLearnerRepository.GetAllDasLearners
{
    public class WhenCalled
    {
        private readonly string _transientConnectionString = ConnectionStringFactory.GetTransientConnectionString();

        private IDasLearnerRepository _dasLearnerRepository;

        [SetUp]
        public void Arrange()
        {
            Database.Clean(_transientConnectionString);

            _dasLearnerRepository = new DataLock.Data.Repositories.DasLearnerRepository(_transientConnectionString);
        }

        [Test]
        public void ThenNoDasLearnersReturnedForNoEntriesInTheDatabase()
        {
            // Act
            var dasLearners = _dasLearnerRepository.GetAllDasLearners();

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
            var dasLearners = _dasLearnerRepository.GetAllDasLearners();

            // Assert
            Assert.IsNotNull(dasLearners);
            Assert.AreEqual(9, dasLearners.Count());
        }
    }
}
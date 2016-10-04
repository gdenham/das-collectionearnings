using System.Linq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.CommitmentRepository
{
    public class WhenGetAllCommitmentsCalled
    {
        private readonly CommitmentEntity[] _commitments =
        {
            new CommitmentBuilder().Build(),
            new CommitmentBuilder().WithCommitmentId("C-002").WithUln(1000000027).WithAgreedCost(30000.00m).Build()
        };

        private ICommitmentRepository _commitmentRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _commitmentRepository = new DataLock.Infrastructure.Data.Repositories.CommitmentRepository(GlobalTestContext.Instance.ConnectionString);
        }

        [Test]
        public void ThenNoCommitmentsReturnedNoEntriesInTheDatabase()
        {
            // Act
            var commitments = _commitmentRepository.GetAllCommitments();

            // Assert
            Assert.IsNotNull(commitments);
            Assert.AreEqual(0, commitments.Count());
        }

        [Test]
        public void ThenCommitmentReturnedForOneEntryInTheDatabase()
        {
            // Arrange
            TestDataHelper.AddCommitment(_commitments[0]);

            // Act
            var response = _commitmentRepository.GetAllCommitments();

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count());
        }

        [Test]
        public void ThenCommitmentsReturnedForMultipleEntriesInTheDatabase()
        {
            // Arrange
            TestDataHelper.AddCommitment(_commitments[0]);
            TestDataHelper.AddCommitment(_commitments[1]);

            // Act
            var response = _commitmentRepository.GetAllCommitments();

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Count());
        }
    }
}

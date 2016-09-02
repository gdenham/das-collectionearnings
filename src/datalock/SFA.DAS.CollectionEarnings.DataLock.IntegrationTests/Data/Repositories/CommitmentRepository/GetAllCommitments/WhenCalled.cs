using System.Linq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.Data.Repositories;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Data.Repositories.CommitmentRepository.GetAllCommitments
{
    public class WhenCalled
    {
        private readonly string _transientConnectionString = ConnectionStringFactory.GetTransientConnectionString();

        private readonly Commitment[] _commitments =
        {
            new CommitmentBuilder().Build(),
            new CommitmentBuilder().WithCommitmentId("C-002").WithUln(1000000027).WithAgreedCost(30000.00m).Build()
        };

        private ICommitmentRepository _commitmentRepository;

        [SetUp]
        public void Arrange()
        {
            Database.Clean(_transientConnectionString);

            _commitmentRepository = new DataLock.Data.Repositories.CommitmentRepository(_transientConnectionString);
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
            Database.AddCommitment(_transientConnectionString, _commitments[0]);

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
            Database.AddCommitment(_transientConnectionString, _commitments[0]);
            Database.AddCommitment(_transientConnectionString, _commitments[1]);

            // Act
            var response = _commitmentRepository.GetAllCommitments();

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Count());
        }
    }
}

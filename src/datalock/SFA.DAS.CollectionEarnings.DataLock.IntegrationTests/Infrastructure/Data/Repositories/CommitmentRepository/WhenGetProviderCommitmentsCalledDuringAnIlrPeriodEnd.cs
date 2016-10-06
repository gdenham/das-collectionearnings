using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.CommitmentRepository
{
    public class WhenGetProviderCommitmentsCalledDuringAnIlrPeriodEnd
    {
        private readonly long _ukprn = 10007459;
        private readonly long _ukprnNoCommitments = 10007458;

        private readonly CommitmentEntity[] _commitments =
        {
            new CommitmentEntityBuilder().Build(),
            new CommitmentEntityBuilder().WithCommitmentId("C-002").WithUln(1000000027).WithAgreedCost(30000.00m).Build()
        };

        private ICommitmentRepository _commitmentRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.PeriodEndClean();

            _commitmentRepository = new DataLock.Infrastructure.Data.Repositories.CommitmentRepository(GlobalTestContext.Instance.PeriodEndConnectionString);
        }

        [Test]
        public void ThenNoCommitmentsReturnedNoEntriesInTheDatabase()
        {
            // Act
            var commitments = _commitmentRepository.GetProviderCommitments(_ukprn);

            // Assert
            Assert.IsNotNull(commitments);
            Assert.AreEqual(0, commitments.Length);
        }

        [Test]
        public void ThenNoCommitmentsReturnedForAUkprnWithNoEntriesInTheDatabase()
        {
            // Arrange
            TestDataHelper.PeriodEndAddCommitment(_commitments[0]);
            TestDataHelper.PeriodEndAddCommitment(_commitments[1]);

            // Act
            var response = _commitmentRepository.GetProviderCommitments(_ukprnNoCommitments);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(0, response.Length);
        }

        [Test]
        public void ThenCommitmentReturnedForOneEntryInTheDatabase()
        {
            // Arrange
            TestDataHelper.PeriodEndAddCommitment(_commitments[0]);

            // Act
            var response = _commitmentRepository.GetProviderCommitments(_ukprn);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Length);
        }

        [Test]
        public void ThenCommitmentsReturnedForMultipleEntriesInTheDatabase()
        {
            // Arrange
            TestDataHelper.PeriodEndAddCommitment(_commitments[0]);
            TestDataHelper.PeriodEndAddCommitment(_commitments[1]);

            // Act
            var response = _commitmentRepository.GetProviderCommitments(_ukprn);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Length);
        }
    }
}
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.LearnerCommitmentRepository
{
    public class WhenAddLearnerCommitmentsCalled
    {
        private ILearnerCommitmentRepository _learnerCommitmentRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _learnerCommitmentRepository = new DataLock.Infrastructure.Data.Repositories.LearnerCommitmentRepository(GlobalTestContext.Instance.ConnectionString);
        }

        [Test]
        public void ThenLearnerCommitmentsAreAddedSuccessfully()
        {
            // Arrange
            var learnerCommitments = new[]
            {
                new LearnerCommitmentEntityBuilder().Build(),
                new LearnerCommitmentEntityBuilder().WithLearnRefNumber("Lrn002").WithCommitmentId("C-002").Build(),
                new LearnerCommitmentEntityBuilder().WithLearnRefNumber("Lrn003").WithAimSeqNumber(2).WithCommitmentId("C-003").Build()
            };

            // Act
            _learnerCommitmentRepository.AddLearnerCommitments(learnerCommitments);

            // Assert
            var learnerCommitmentEntities = TestDataHelper.GetLearnerCommitmentEntities();

            Assert.IsNotNull(learnerCommitmentEntities);
            Assert.AreEqual(3, learnerCommitmentEntities.Length);
        }
    }
}
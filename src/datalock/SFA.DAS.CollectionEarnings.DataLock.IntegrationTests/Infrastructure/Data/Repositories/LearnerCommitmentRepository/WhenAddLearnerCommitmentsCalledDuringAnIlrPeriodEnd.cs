﻿using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.LearnerCommitmentRepository
{
    public class WhenAddLearnerCommitmentsCalledDuringAnIlrPeriodEnd
    {
        private ILearnerCommitmentRepository _learnerCommitmentRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _learnerCommitmentRepository = new DataLock.Infrastructure.Data.Repositories.LearnerCommitmentRepository(GlobalTestContext.Instance.PeriodEndConnectionString);
        }

        [Test]
        public void ThenLearnerCommitmentsAreAddedSuccessfully()
        {
            // Arrange
            var learnerCommitments = new[]
            {
                new LearnerCommitmentEntityBuilder().Build(),
                new LearnerCommitmentEntityBuilder().WithLearnRefNumber("Lrn002").WithCommitmentId(2).Build(),
                new LearnerCommitmentEntityBuilder().WithLearnRefNumber("Lrn003").WithAimSeqNumber(2).WithCommitmentId(2).Build()
            };

            // Act
            _learnerCommitmentRepository.AddLearnerCommitments(learnerCommitments);

            // Assert
            var learnerCommitmentEntities = TestDataHelper.PeriodEndGetLearnerAndCommitmentMatches();

            Assert.IsNotNull(learnerCommitmentEntities);
            Assert.AreEqual(3, learnerCommitmentEntities.Length);
        }
    }
}
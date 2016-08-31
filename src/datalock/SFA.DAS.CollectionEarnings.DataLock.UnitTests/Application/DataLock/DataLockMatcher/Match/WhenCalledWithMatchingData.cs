using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Common.Tests.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.DataLockMatcher.Match
{
    public class WhenCalledWithMatchingData
    {
        [Test]
        public void ThenExpectingTrueForUkprnLevel()
        {
            // Arrange
            var commitment = new CommitmentBuilder().Build();
            var learner = new DasLearnerBuilder().Build();

            // Act
            var result = CollectionEarnings.DataLock.Application.DataLock.DataLockMatcher.Match(commitment, learner, DataLockMatchLevel.Ukprn);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ThenExpectingTrueForUlnLevel()
        {
            // Arrange
            var commitment = new CommitmentBuilder().Build();
            var learner = new DasLearnerBuilder().Build();

            // Act
            var result = CollectionEarnings.DataLock.Application.DataLock.DataLockMatcher.Match(commitment, learner, DataLockMatchLevel.Uln);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ThenExpectingTrueForStandardLevel()
        {
            // Arrange
            var commitment = new CommitmentBuilder().WithStandardCode(999).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build();
            var learner = new DasLearnerBuilder().WithStdCode(999).WithProgType(null).WithFworkCode(null).WithPwayCode(null).Build();

            // Act
            var result = CollectionEarnings.DataLock.Application.DataLock.DataLockMatcher.Match(commitment, learner, DataLockMatchLevel.Standard);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ThenExpectingTrueForFrameworkLevel()
        {
            // Arrange
            var commitment = new CommitmentBuilder().Build();
            var learner = new DasLearnerBuilder().Build();

            // Act
            var result = CollectionEarnings.DataLock.Application.DataLock.DataLockMatcher.Match(commitment, learner, DataLockMatchLevel.Framework);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ThenExpectingTrueForProgrammeLevel()
        {
            // Arrange
            var commitment = new CommitmentBuilder().Build();
            var learner = new DasLearnerBuilder().Build();

            // Act
            var result = CollectionEarnings.DataLock.Application.DataLock.DataLockMatcher.Match(commitment, learner, DataLockMatchLevel.Programme);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ThenExpectingTrueForPathwayLevel()
        {
            // Arrange
            var commitment = new CommitmentBuilder().Build();
            var learner = new DasLearnerBuilder().Build();

            // Act
            var result = CollectionEarnings.DataLock.Application.DataLock.DataLockMatcher.Match(commitment, learner, DataLockMatchLevel.Pathway);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ThenExpectingTrueForPriceLevelWithStandard()
        {
            // Arrange
            var commitment = new CommitmentBuilder().WithStandardCode(999).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build();
            var learner = new DasLearnerBuilder().WithStdCode(999).WithProgType(null).WithFworkCode(null).WithPwayCode(null).Build();

            // Act
            var result = CollectionEarnings.DataLock.Application.DataLock.DataLockMatcher.Match(commitment, learner, DataLockMatchLevel.Price);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ThenExpectingTrueForPriceLevelWithFramework()
        {
            // Arrange
            var commitment = new CommitmentBuilder().Build();
            var learner = new DasLearnerBuilder().Build();

            // Act
            var result = CollectionEarnings.DataLock.Application.DataLock.DataLockMatcher.Match(commitment, learner, DataLockMatchLevel.Price);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
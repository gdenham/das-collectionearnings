using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.DataLockMatcher.Match
{
    public class WhenCalledWithMismatchingData
    {
        private static readonly object[] LearnersWithMismatchingUln =
        {
            new object[] {new DasLearnerBuilder().WithUln(999).Build()},
            new object[] {new DasLearnerBuilder().WithUln(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingFramework =
        {
            new object[] {new DasLearnerBuilder().WithFworkCode(999).Build()},
            new object[] {new DasLearnerBuilder().WithFworkCode(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingProgramme =
        {
            new object[] {new DasLearnerBuilder().WithProgType(999).Build()},
            new object[] {new DasLearnerBuilder().WithProgType(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingPathway =
        {
            new object[] {new DasLearnerBuilder().WithPwayCode(999).Build()},
            new object[] {new DasLearnerBuilder().WithPwayCode(null).Build()}
        };

        private static readonly object[] LearnersWithMatchingStandardAndMismatchingPrice =
        {
            new object[] {new DasLearnerBuilder().WithStdCode(999).WithTbFinAmount(999).Build()},
            new object[] {new DasLearnerBuilder().WithStdCode(999).WithTbFinAmount(null).Build()}
        };

        private static readonly object[] LearnersWithMatchingFrameworkAndMismatchingPrice =
        {
            new object[] {new DasLearnerBuilder().WithTbFinAmount(999).Build()},
            new object[] {new DasLearnerBuilder().WithTbFinAmount(null).Build()}
        };

        [Test]
        public void ThenExpectingFalseForUkprnLevel()
        {
            // Arrange
            var commitment = new CommitmentBuilder().Build();
            var learner = new DasLearnerBuilder().WithUkprn(999).Build();

            // Act
            var result = CollectionEarnings.DataLock.Application.DataLock.DataLockMatcher.MatchUkprn(commitment, learner);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMismatchingUln))]
        public void ThenExpectingFalseForUlnLevel(Data.Entities.DasLearner learner)
        {
            // Arrange
            var commitment = new CommitmentBuilder().Build();

            // Act
            var result = CollectionEarnings.DataLock.Application.DataLock.DataLockMatcher.MatchUln(commitment, learner);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ThenExpectingFalseForStandardLevel()
        {
            // Arrange
            var commitment = new CommitmentBuilder().WithStandardCode(999).Build();
            var learner = new DasLearnerBuilder().WithStdCode(998).Build();

            // Act
            var result = CollectionEarnings.DataLock.Application.DataLock.DataLockMatcher.MatchStandard(commitment, learner);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMismatchingFramework))]
        public void ThenExpectingFalseForFrameworkLevel(Data.Entities.DasLearner learner)
        {
            // Arrange
            var commitment = new CommitmentBuilder().Build();

            // Act
            var result = CollectionEarnings.DataLock.Application.DataLock.DataLockMatcher.MatchFramework(commitment, learner);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMismatchingProgramme))]
        public void ThenExpectingFalseForProgrammeLevel(Data.Entities.DasLearner learner)
        {
            // Arrange
            var commitment = new CommitmentBuilder().Build();

            // Act
            var result = CollectionEarnings.DataLock.Application.DataLock.DataLockMatcher.MatchProgramme(commitment, learner);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMismatchingPathway))]
        public void ThenExpectingFalseForPathwayLevel(Data.Entities.DasLearner learner)
        {
            // Arrange
            var commitment = new CommitmentBuilder().Build();

            // Act
            var result = CollectionEarnings.DataLock.Application.DataLock.DataLockMatcher.MatchPathway(commitment, learner);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMatchingStandardAndMismatchingPrice))]
        public void ThenExpectingFalseForPriceLevelWithStandard(Data.Entities.DasLearner learner)
        {
            // Arrange
            var commitment = new CommitmentBuilder().WithStandardCode(999).Build();

            // Act
            var result = CollectionEarnings.DataLock.Application.DataLock.DataLockMatcher.MatchPrice(commitment, learner);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMatchingFrameworkAndMismatchingPrice))]
        public void ThenExpectingFalseForPriceLevelWithFramework(Data.Entities.DasLearner learner)
        {
            // Arrange
            var commitment = new CommitmentBuilder().Build();

            // Act
            var result = CollectionEarnings.DataLock.Application.DataLock.DataLockMatcher.MatchPrice(commitment, learner);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
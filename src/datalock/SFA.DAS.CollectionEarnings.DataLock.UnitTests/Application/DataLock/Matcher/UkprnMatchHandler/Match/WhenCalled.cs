using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.Matcher.UkprnMatchHandler.Match
{
    public class WhenCalled
    {
        private CollectionEarnings.DataLock.Application.DataLock.Matcher.UkprnMatchHandler _matcher;
        private Mock<MatchHandler> _nextMatcher;

        [SetUp]
        public void Arrange()
        {
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.UkprnMatchHandler();
            _nextMatcher = new Mock<MatchHandler>();

            _nextMatcher
                .Setup(m => m.Match(It.IsAny<List<CollectionEarnings.DataLock.Application.Commitment.Commitment>>(), It.IsAny<CollectionEarnings.DataLock.Application.Learner.Learner>()))
                .Returns(new MatchResult { ErrorCode = string.Empty });

            _matcher.SetNextMatchHandler(_nextMatcher.Object);
        }

        [Test]
        public void ThenNextMatcherInChainIsExecutedForMatchingDataProvided()
        {
            // Arrange
            var commitments = new List<CollectionEarnings.DataLock.Application.Commitment.Commitment>
            {
                new CommitmentBuilder().Build(),
                new CommitmentBuilder().Withukprn(999).Build()
            };

            var learner = new LearnerBuilder().Build();

            // Act
            var matchResult = _matcher.Match(commitments, learner);

            // Assert
            Assert.IsEmpty(matchResult.ErrorCode);
            _nextMatcher.Verify(
                m =>
                    m.Match(It.Is<List<CollectionEarnings.DataLock.Application.Commitment.Commitment>>(x => x[0].Equals(commitments[0])),
                        It.IsAny<CollectionEarnings.DataLock.Application.Learner.Learner>()),
                Times.Once());
        }

        [Test]
        public void ThenErrorCodeReturnedForMismatchingDataProvided()
        {
            // Arrange
            var commitments = new List<CollectionEarnings.DataLock.Application.Commitment.Commitment>
            {
                new CommitmentBuilder().Withukprn(998).Build(),
                new CommitmentBuilder().Withukprn(999).Build()
            };

            var learner = new LearnerBuilder().Build();

            // Act
            var matchResult = _matcher.Match(commitments, learner);

            // Assert
            Assert.AreEqual(DataLockErrorCodes.MismatchingUkprn, matchResult.ErrorCode);
            _nextMatcher.Verify(
                m =>
                    m.Match(It.IsAny<List<CollectionEarnings.DataLock.Application.Commitment.Commitment>>(), It.IsAny<CollectionEarnings.DataLock.Application.Learner.Learner>()),
                Times.Never());
        }
    }
}
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.Matcher.UlnMatchHandler.Match
{
    public class WhenCalled
    {
        private CollectionEarnings.DataLock.Application.DataLock.Matcher.UlnMatchHandler _matcher;
        private Mock<MatchHandler> _nextMatcher;

        [SetUp]
        public void Arrange()
        {
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.UlnMatchHandler();
            _nextMatcher = new Mock<MatchHandler>();

            _nextMatcher
                .Setup(m => m.Match(It.IsAny<List<Data.Entities.Commitment>>(), It.IsAny<Data.Entities.DasLearner>()))
                .Returns(string.Empty);

            _matcher.SetNextMatchHandler(_nextMatcher.Object);
        }

        [Test]
        public void ThenNextMatcherInChainIsExecutedForMatchingDataProvided()
        {
            // Arrange
            var commitments = new List<Data.Entities.Commitment>
            {
                new CommitmentBuilder().Build(),
                new CommitmentBuilder().WithUln(999).Build()
            };

            var learner = new DasLearnerBuilder().Build();

            // Act
            var matchResult = _matcher.Match(commitments, learner);

            // Assert
            Assert.IsEmpty(matchResult);
            _nextMatcher.Verify(
                m =>
                    m.Match(It.Is<List<Data.Entities.Commitment>>(x => x[0].Equals(commitments[0])),
                        It.IsAny<Data.Entities.DasLearner>()),
                Times.Once());
        }

        [Test]
        public void ThenErrorCodeReturnedForMismatchingDataProvided()
        {
            // Arrange
            var commitments = new List<Data.Entities.Commitment>
            {
                new CommitmentBuilder().WithUln(998).Build(),
                new CommitmentBuilder().WithUln(999).Build()
            };

            var learner = new DasLearnerBuilder().Build();

            // Act
            var matchResult = _matcher.Match(commitments, learner);

            // Assert
            Assert.AreEqual(DataLockErrorCodes.MismatchingUln, matchResult);
            _nextMatcher.Verify(
                m =>
                    m.Match(It.IsAny<List<Data.Entities.Commitment>>(), It.IsAny<Data.Entities.DasLearner>()),
                Times.Never());
        }
    }
}
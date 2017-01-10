using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.Matcher.FrameworkMatchHandler.Match
{
    public class WhenCalled
    {
        private CollectionEarnings.DataLock.Application.DataLock.Matcher.FrameworkMatchHandler _matcher;
        private Mock<MatchHandler> _nextMatcher;

        [SetUp]
        public void Arrange()
        {
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.FrameworkMatchHandler();
            _nextMatcher = new Mock<MatchHandler>();

            _nextMatcher
                .Setup(m => m.Match(It.IsAny<List<CollectionEarnings.DataLock.Application.Commitment.Commitment>>(), It.IsAny<CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode>()))
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
                new CommitmentBuilder().WithFrameworkCode(999).Build()
            };

            var priceEpisode = new PriceEpisodeBuilder().Build();

            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode);

            // Assert
            Assert.IsEmpty(matchResult.ErrorCode);
            _nextMatcher.Verify(
                m =>
                    m.Match(It.Is<List<CollectionEarnings.DataLock.Application.Commitment.Commitment>>(x => x[0].Equals(commitments[0])),
                        It.IsAny<CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode>()),
                Times.Once());
        }

        [Test]
        public void ThenErrorCodeReturnedForMismatchingDataProvided()
        {
            // Arrange
            var commitments = new List<CollectionEarnings.DataLock.Application.Commitment.Commitment>
            {
                new CommitmentBuilder().WithFrameworkCode(998).Build(),
                new CommitmentBuilder().WithFrameworkCode(999).Build()
            };

            var priceEpisode = new PriceEpisodeBuilder().Build();

            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode);

            // Assert
            Assert.AreEqual(DataLockErrorCodes.MismatchingFramework, matchResult.ErrorCode);
            _nextMatcher.Verify(
                m =>
                    m.Match(It.IsAny<List<CollectionEarnings.DataLock.Application.Commitment.Commitment>>(), It.IsAny<CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode>()),
                Times.Never());
        }
    }
}
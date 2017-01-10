using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.Matcher.MultipleMatchHandler.Match
{
    public class WhenCalled
    {
        private CollectionEarnings.DataLock.Application.DataLock.Matcher.MultipleMatchHandler _matcher;

        [SetUp]
        public void Arrange()
        {
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.MultipleMatchHandler();
        }

        [Test]
        public void ThenNullReturnedForSingleMatchingDataProvided()
        {
            // Arrange
            var commitments = new List<CollectionEarnings.DataLock.Application.Commitment.Commitment>
            {
                new CommitmentBuilder().Build()
            };

            var priceEpisode = new PriceEpisodeBuilder().Build();

            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode);

            // Assert
            Assert.IsNull(matchResult.ErrorCode);
        }

        [Test]
        public void ThenErrorCodeReturnedForMultipleMatchingDataProvided()
        {
            // Arrange
            var commitments = new List<CollectionEarnings.DataLock.Application.Commitment.Commitment>
            {
                new CommitmentBuilder().Build(),
                new CommitmentBuilder().WithCommitmentId(2).Build()
            };

            var priceEpisode = new PriceEpisodeBuilder().Build();

            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode);

            // Assert
            Assert.AreEqual(DataLockErrorCodes.MultipleMatches, matchResult.ErrorCode);
        }
    }
}
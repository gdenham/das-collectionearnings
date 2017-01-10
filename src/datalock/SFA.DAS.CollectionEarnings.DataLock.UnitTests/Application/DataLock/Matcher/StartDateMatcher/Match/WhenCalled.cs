using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.Matcher.StartDateMatcher.Match
{
    public class WhenCalled
    {
        private static readonly object[] MatchingStartDates =
        {
            new object[] {new DateTime(2016, 9, 15), new DateTime(2016, 9, 15)},
            new object[] {new DateTime(2016, 9, 1), new DateTime(2016, 9, 15)}
        };

        private static readonly object[] MismatchingStartDates =
        {
            new object[] {new DateTime(2016, 9, 16), new DateTime(2016, 9, 15)},
            new object[] {new DateTime(2016, 10, 1), new DateTime(2016, 9, 15)}
        };

        private CollectionEarnings.DataLock.Application.DataLock.Matcher.StartDateMatcher _matcher;
        private Mock<MatchHandler> _nextMatcher;

        [SetUp]
        public void Arrange()
        {
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.StartDateMatcher();
            _nextMatcher = new Mock<MatchHandler>();

            _nextMatcher
                .Setup(m => m.Match(It.IsAny<List<CollectionEarnings.DataLock.Application.Commitment.Commitment>>(), It.IsAny<CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode>()))
                .Returns(new MatchResult { ErrorCode = string.Empty });

            _matcher.SetNextMatchHandler(_nextMatcher.Object);
        }

        [Test]
        [TestCaseSource(nameof(MatchingStartDates))]
        public void ThenNextMatcherInChainIsExecutedForMatchingDataProvided(DateTime commitmentStartDate, DateTime learnerStartDate)
        {
            // Arrange
            var commitments = new List<CollectionEarnings.DataLock.Application.Commitment.Commitment>
            {
                new CommitmentBuilder().WithStartDate(commitmentStartDate).Build()
            };

            var priceEpisode = new PriceEpisodeBuilder().WithLearnStartDate(learnerStartDate).Build();

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
        [TestCaseSource(nameof(MismatchingStartDates))]
        public void ThenErrorCodeReturnedForMismatchingDataProvided(DateTime commitmentStartDate, DateTime learnerStartDate)
        {
            // Arrange
            var commitments = new List<CollectionEarnings.DataLock.Application.Commitment.Commitment>
            {
                new CommitmentBuilder().WithStartDate(commitmentStartDate).Build()
            };

            var priceEpisode = new PriceEpisodeBuilder().WithLearnStartDate(learnerStartDate).Build();

            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode);

            // Assert
            Assert.AreEqual(DataLockErrorCodes.EarlierStartDate, matchResult.ErrorCode);
            _nextMatcher.Verify(
                m =>
                    m.Match(It.IsAny<List<CollectionEarnings.DataLock.Application.Commitment.Commitment>>(), It.IsAny<CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode>()),
                Times.Never());
        }
    }
}
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Enums;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.Matcher.IsPayableMatchHandler
{
    public class WhenMatching
    {
        private CollectionEarnings.DataLock.Application.DataLock.Matcher.IsPayableMatchHandler _matcher;
        private Mock<MatchHandler> _nextMatcher;

        [SetUp]
        public void Arrange()
        {
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.IsPayableMatchHandler();
            _nextMatcher = new Mock<MatchHandler>();

            _nextMatcher
                .Setup(m => m.Match(It.IsAny<List<CollectionEarnings.DataLock.Application.Commitment.Commitment>>(), It.IsAny<CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode>()))
                .Returns(new MatchResult { ErrorCode = string.Empty });

            _matcher.SetNextMatchHandler(_nextMatcher.Object);
        }

        [Test]
        [TestCase(PaymentStatus.Active)]
        [TestCase(PaymentStatus.Completed)]
        public void ThenNextMatcherInChainIsExecutedForMatchingDataProvided(PaymentStatus payablePaymentStatus)
        {
            // Arrange
            var commitments = new List<CollectionEarnings.DataLock.Application.Commitment.Commitment>
            {
                new CommitmentBuilder().WithPaymentStatus(payablePaymentStatus).Build(),
                new CommitmentBuilder().WithPaymentStatus(PaymentStatus.Paused).Build()
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
        [TestCase(PaymentStatus.PendingApproval)]
        [TestCase(PaymentStatus.Paused)]
        [TestCase(PaymentStatus.Cancelled)]
        [TestCase(PaymentStatus.Deleted)]
        public void ThenErrorCodeReturnedForMismatchingDataProvided(PaymentStatus notPayablePaymentStatus)
        {
            // Arrange
            var commitments = new List<CollectionEarnings.DataLock.Application.Commitment.Commitment>
            {
                new CommitmentBuilder().WithPaymentStatus(notPayablePaymentStatus).Build()
            };

            var priceEpisode = new PriceEpisodeBuilder().Build();

            // Act
            var matchResult = _matcher.Match(commitments, priceEpisode);

            // Assert
            Assert.AreEqual(DataLockErrorCodes.NotPayable, matchResult.ErrorCode);
            _nextMatcher.Verify(
                m =>
                    m.Match(It.IsAny<List<CollectionEarnings.DataLock.Application.Commitment.Commitment>>(), It.IsAny<CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode>()),
                Times.Never());
        }
    }
}
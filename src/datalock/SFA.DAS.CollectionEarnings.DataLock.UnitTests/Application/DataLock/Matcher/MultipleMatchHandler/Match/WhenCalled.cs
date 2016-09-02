using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

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
            var commitments = new List<Data.Entities.Commitment>
            {
                new CommitmentBuilder().Build()
            };

            var learner = new DasLearnerBuilder().Build();

            // Act
            var matchResult = _matcher.Match(commitments, learner);

            // Assert
            Assert.IsNull(matchResult);
        }

        [Test]
        public void ThenErrorCodeReturnedForMultipleMatchingDataProvided()
        {
            // Arrange
            var commitments = new List<Data.Entities.Commitment>
            {
                new CommitmentBuilder().Build(),
                new CommitmentBuilder().WithCommitmentId("C-002").Build()
            };

            var learner = new DasLearnerBuilder().Build();

            // Act
            var matchResult = _matcher.Match(commitments, learner);

            // Assert
            Assert.AreEqual(DataLockErrorCodes.MultipleMatches, matchResult);
        }
    }
}
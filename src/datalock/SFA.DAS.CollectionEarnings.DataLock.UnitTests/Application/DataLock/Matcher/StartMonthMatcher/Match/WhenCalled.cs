﻿using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.Matcher.StartMonthMatcher.Match
{
    public class WhenCalled
    {
        private CollectionEarnings.DataLock.Application.DataLock.Matcher.StartMonthMatcher _matcher;
        private Mock<MatchHandler> _nextMatcher;

        [SetUp]
        public void Arrange()
        {
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.StartMonthMatcher();
            _nextMatcher = new Mock<MatchHandler>();

            _nextMatcher
                .Setup(m => m.Match(It.IsAny<List<Infrastructure.Data.Entities.CommitmentEntity>>(), It.IsAny<Infrastructure.Data.Entities.LearnerEntity>()))
                .Returns(new MatchResult { ErrorCode = string.Empty });

            _matcher.SetNextMatchHandler(_nextMatcher.Object);
        }

        [Test]
        public void ThenNextMatcherInChainIsExecutedForMatchingDataProvided()
        {
            // Arrange
            var commitments = new List<Infrastructure.Data.Entities.CommitmentEntity>
            {
                new CommitmentBuilder().Build(),
                new CommitmentBuilder().WithStartDate(new DateTime(2016, 10, 1)).Build()
            };

            var learner = new LearnerEntityBuilder().Build();

            // Act
            var matchResult = _matcher.Match(commitments, learner);

            // Assert
            Assert.IsEmpty(matchResult.ErrorCode);
            _nextMatcher.Verify(
                m =>
                    m.Match(It.Is<List<Infrastructure.Data.Entities.CommitmentEntity>>(x => x[0].Equals(commitments[0])),
                        It.IsAny<Infrastructure.Data.Entities.LearnerEntity>()),
                Times.Once());
        }

        [Test]
        public void ThenErrorCodeReturnedForMismatchingDataProvided()
        {
            // Arrange
            var commitments = new List<Infrastructure.Data.Entities.CommitmentEntity>
            {
                new CommitmentBuilder().WithStartDate(new DateTime(2016, 10, 1)).Build(),
                new CommitmentBuilder().WithStartDate(new DateTime(2016, 11, 1)).Build()
            };

            var learner = new LearnerEntityBuilder().Build();

            // Act
            var matchResult = _matcher.Match(commitments, learner);

            // Assert
            Assert.AreEqual(DataLockErrorCodes.EarlierStartMonth, matchResult.ErrorCode);
            _nextMatcher.Verify(
                m =>
                    m.Match(It.IsAny<List<Infrastructure.Data.Entities.CommitmentEntity>>(), It.IsAny<Infrastructure.Data.Entities.LearnerEntity>()),
                Times.Never());
        }
    }
}
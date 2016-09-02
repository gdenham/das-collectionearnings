using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.Matcher.StandardMatchHandler.Match
{
    public class WhenCalled
    {
        private CollectionEarnings.DataLock.Application.DataLock.Matcher.StandardMatchHandler _matcher;
        private Mock<MatchHandler> _nextMatcher;

        [SetUp]
        public void Arrange()
        {
            _matcher = new CollectionEarnings.DataLock.Application.DataLock.Matcher.StandardMatchHandler();
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
                new CommitmentBuilder().WithStandardCode(1).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build(),
                new CommitmentBuilder().WithStandardCode(999).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build()
            };

            var learner = new DasLearnerBuilder().WithStdCode(1).WithProgType(null).WithFworkCode(null).WithPwayCode(null).Build();

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
                new CommitmentBuilder().WithStandardCode(998).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build(),
                new CommitmentBuilder().WithStandardCode(999).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build()
            };

            var learner = new DasLearnerBuilder().WithStdCode(1).WithProgType(null).WithFworkCode(null).WithPwayCode(null).Build();

            // Act
            var matchResult = _matcher.Match(commitments, learner);

            // Assert
            Assert.AreEqual(DataLockErrorCodes.MismatchingStandard, matchResult);
            _nextMatcher.Verify(
                m =>
                    m.Match(It.IsAny<List<Data.Entities.Commitment>>(), It.IsAny<Data.Entities.DasLearner>()),
                Times.Never());
        }
    }
}
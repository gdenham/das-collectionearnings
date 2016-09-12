using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.GetDataLockFailuresQuery;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.GetDataLockFailuresQuery.GetDataLockFailuresQueryHandler.Handle
{
    public class WhenCalled
    {
        private static readonly object[] LearnersWithMismatchingUln =
        {
            new object[] {new DasLearnerBuilder().WithUln(1000000018).Build()},
            new object[] {new DasLearnerBuilder().WithUln(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingFramework =
        {
            new object[] {new DasLearnerBuilder().WithFrameworkCode(999).Build()},
            new object[] {new DasLearnerBuilder().WithFrameworkCode(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingProgramme =
        {
            new object[] {new DasLearnerBuilder().WithProgrammeType(999).Build()},
            new object[] {new DasLearnerBuilder().WithProgrammeType(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingPathway =
        {
            new object[] {new DasLearnerBuilder().WithPathwayCode(999).Build()},
            new object[] {new DasLearnerBuilder().WithPathwayCode(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingPrice =
        {
            new object[] {new DasLearnerBuilder().WithNegotiatedPrice(999).Build()},
            new object[] {new DasLearnerBuilder().WithNegotiatedPrice(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingStartDates =
       {
            new object[] {new DasLearnerBuilder().WithLearnStartDate(new DateTime(2016, 8, 31)).Build()},
            new object[] {new DasLearnerBuilder().WithLearnStartDate(null).Build()}
        };

        private GetDataLockFailuresQueryRequest _request;
        private readonly CollectionEarnings.DataLock.Application.DataLock.GetDataLockFailuresQuery.GetDataLockFailuresQueryHandler _handler = new CollectionEarnings.DataLock.Application.DataLock.GetDataLockFailuresQuery.GetDataLockFailuresQueryHandler();

        [Test]
        public void ThenNoErrorExpectedForMatchingCommitmentAndLearnerData()
        {
            // Arrange
            _request = new GetDataLockFailuresQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                DasLearners = new[]
                {
                    new DasLearnerBuilder().Build()
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(0, response.Items.Count());
        }

        [Test]
        public void ThenErrorExpectedForNoUkprnMatch()
        {
            // Arrange
            _request = new GetDataLockFailuresQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                DasLearners = new[]
                {
                    new DasLearnerBuilder().WithUkprn(10007458).Build()
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.Items.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingUkprn));
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMismatchingUln))]
        public void ThenErrorExpectedForNoUlnMatch(Data.Entities.DasLearner dasLearner)
        {
            // Arrange
            _request = new GetDataLockFailuresQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                DasLearners = new[]
                {
                    dasLearner
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.Items.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingUln));
        }

        [Test]
        public void ThenErrorExpectedForNoStandardMatch()
        {
            // Arrange
            _request = new GetDataLockFailuresQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().WithStandardCode(999).Build()
                },
                DasLearners = new[]
                {
                    new DasLearnerBuilder().WithStandardCode(998).Build()
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.Items.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingStandard));
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMismatchingFramework))]
        public void ThenErrorExpectedForNoFrameworkMatch(Data.Entities.DasLearner dasLearner)
        {
            // Arrange
            _request = new GetDataLockFailuresQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                DasLearners = new[]
                {
                    dasLearner
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.Items.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingFramework));
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMismatchingProgramme))]
        public void ThenErrorExpectedForNoProgrammeMatch(Data.Entities.DasLearner dasLearner)
        {
            // Arrange
            _request = new GetDataLockFailuresQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                DasLearners = new[]
                {
                    dasLearner
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.Items.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingProgramme));
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMismatchingPathway))]
        public void ThenErrorExpectedForNoPathwayMatch(Data.Entities.DasLearner dasLearner)
        {
            // Arrange
            _request = new GetDataLockFailuresQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                DasLearners = new[]
                {
                    dasLearner
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.Items.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingPathway));
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMismatchingPrice))]
        public void ThenErrorExpectedForNoPriceMatch(Data.Entities.DasLearner dasLearner)
        {
            // Arrange
            _request = new GetDataLockFailuresQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                DasLearners = new[]
                {
                    dasLearner
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.Items.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingPrice));
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMismatchingStartDates))]
        public void ThenErrorExpectedForNoStartDateOrEarlierStartDate(Data.Entities.DasLearner dasLearner)
        {
            // Arrange
            _request = new GetDataLockFailuresQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                DasLearners = new[]
                {
                    dasLearner
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.Items.Count(ve => ve.RuleId == DataLockErrorCodes.EarlierStartMonth));
        }

        [Test]
        public void ThenErrorExpectedForMultiplePriceMatchingCommitments()
        {
            // Arrange
            _request = new GetDataLockFailuresQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build(),
                    new CommitmentBuilder().WithCommitmentId("C-002").Build()
                },
                DasLearners = new[]
                {
                    new DasLearnerBuilder().Build()
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.Items.Count(ve => ve.RuleId == DataLockErrorCodes.MultipleMatches));
        }

        [Test]
        public void ThenMultipleErrorsExpectedForMultipleLearnersProvided()
        {
            // Arrange
            _request = new GetDataLockFailuresQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                DasLearners = new[]
                {
                    new DasLearnerBuilder().Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn002").WithUkprn(10007458).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn003").WithUln(1000000018).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn004").WithUln(null).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn005").WithStandardCode(998).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn006").WithFrameworkCode(999).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn007").WithFrameworkCode(null).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn008").WithProgrammeType(999).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn009").WithProgrammeType(null).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn010").WithPathwayCode(999).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn011").WithPathwayCode(null).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn012").WithNegotiatedPrice(999).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn013").WithNegotiatedPrice(null).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn014").WithLearnStartDate(new DateTime(2016, 8, 31)).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn015").WithLearnStartDate(null).Build()
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(14, response.Items.Count());
        }
    }
}

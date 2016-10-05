using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.RunDataLockValidationQuery.RunDataLockValidationQueryHandler
{
    public class WhenHandling
    {
        private static readonly object[] LearnersWithMismatchingUln =
        {
            new object[] {new LearnerBuilder().WithUln(1000000018).Build()},
            new object[] {new LearnerBuilder().WithUln(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingFramework =
        {
            new object[] {new LearnerBuilder().WithFrameworkCode(999).Build()},
            new object[] {new LearnerBuilder().WithFrameworkCode(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingProgramme =
        {
            new object[] {new LearnerBuilder().WithProgrammeType(999).Build()},
            new object[] {new LearnerBuilder().WithProgrammeType(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingPathway =
        {
            new object[] {new LearnerBuilder().WithPathwayCode(999).Build()},
            new object[] {new LearnerBuilder().WithPathwayCode(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingPrice =
        {
            new object[] {new LearnerBuilder().WithNegotiatedPrice(999).Build()},
            new object[] {new LearnerBuilder().WithNegotiatedPrice(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingStartDates =
       {
            new object[] {new LearnerBuilder().WithLearnStartDate(new DateTime(2016, 8, 31)).Build()},
            new object[] {new LearnerBuilder().WithLearnStartDate(null).Build()}
        };

        private RunDataLockValidationQueryRequest _request;
        private readonly CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery.RunDataLockValidationQueryHandler _handler = new CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery.RunDataLockValidationQueryHandler();

        [Test]
        public void ThenNoErrorExpectedForMatchingCommitmentAndLearnerData()
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                Learners = new[]
                {
                    new LearnerBuilder().Build()
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(0, response.ValidationErrors.Length);
            Assert.AreEqual(1, response.LearnerCommitments.Length);
        }

        [Test]
        public void ThenErrorExpectedForNoUkprnMatch()
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                Learners = new[]
                {
                    new LearnerBuilder().WithUkprn(10007458).Build()
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingUkprn));
            Assert.AreEqual(0, response.LearnerCommitments.Length);
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMismatchingUln))]
        public void ThenErrorExpectedForNoUlnMatch(CollectionEarnings.DataLock.Application.Learner.Learner dasLearner)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                Learners = new[]
                {
                    dasLearner
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingUln));
            Assert.AreEqual(0, response.LearnerCommitments.Length);
        }

        [Test]
        public void ThenErrorExpectedForNoStandardMatch()
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().WithStandardCode(999).Build()
                },
                Learners = new[]
                {
                    new LearnerBuilder().WithStandardCode(998).Build()
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingStandard));
            Assert.AreEqual(0, response.LearnerCommitments.Length);
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMismatchingFramework))]
        public void ThenErrorExpectedForNoFrameworkMatch(CollectionEarnings.DataLock.Application.Learner.Learner dasLearner)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                Learners = new[]
                {
                    dasLearner
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingFramework));
            Assert.AreEqual(0, response.LearnerCommitments.Length);
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMismatchingProgramme))]
        public void ThenErrorExpectedForNoProgrammeMatch(CollectionEarnings.DataLock.Application.Learner.Learner dasLearner)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                Learners = new[]
                {
                    dasLearner
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingProgramme));
            Assert.AreEqual(0, response.LearnerCommitments.Length);
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMismatchingPathway))]
        public void ThenErrorExpectedForNoPathwayMatch(CollectionEarnings.DataLock.Application.Learner.Learner dasLearner)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                Learners = new[]
                {
                    dasLearner
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingPathway));
            Assert.AreEqual(0, response.LearnerCommitments.Length);
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMismatchingPrice))]
        public void ThenErrorExpectedForNoPriceMatch(CollectionEarnings.DataLock.Application.Learner.Learner dasLearner)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                Learners = new[]
                {
                    dasLearner
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.MismatchingPrice));
            Assert.AreEqual(0, response.LearnerCommitments.Length);
        }

        [Test]
        [TestCaseSource(nameof(LearnersWithMismatchingStartDates))]
        public void ThenErrorExpectedForNoStartDateOrEarlierStartDate(CollectionEarnings.DataLock.Application.Learner.Learner dasLearner)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                Learners = new[]
                {
                    dasLearner
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.EarlierStartMonth));
            Assert.AreEqual(0, response.LearnerCommitments.Length);
        }

        [Test]
        public void ThenErrorExpectedForMultiplePriceMatchingCommitments()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentBuilder().Build(),
                new CommitmentBuilder().WithCommitmentId("C-002").Build()
            };

            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = commitments,
                Learners = new[]
                {
                    new LearnerBuilder().Build()
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.MultipleMatches));
            Assert.AreEqual(0, response.LearnerCommitments.Length);
        }

        [Test]
        public void ThenMultipleErrorsExpectedForMultipleLearnersProvided()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentBuilder().Build()
            };

            var learners = new[]
            {
                new LearnerBuilder().Build(),
                new LearnerBuilder().WithLearnRefNumber("Lrn002").WithUkprn(10007458).Build(),
                new LearnerBuilder().WithLearnRefNumber("Lrn003").WithUln(1000000018).Build(),
                new LearnerBuilder().WithLearnRefNumber("Lrn004").WithUln(null).Build(),
                new LearnerBuilder().WithLearnRefNumber("Lrn005").WithStandardCode(998).Build(),
                new LearnerBuilder().WithLearnRefNumber("Lrn006").WithFrameworkCode(999).Build(),
                new LearnerBuilder().WithLearnRefNumber("Lrn007").WithFrameworkCode(null).Build(),
                new LearnerBuilder().WithLearnRefNumber("Lrn008").WithProgrammeType(999).Build(),
                new LearnerBuilder().WithLearnRefNumber("Lrn009").WithProgrammeType(null).Build(),
                new LearnerBuilder().WithLearnRefNumber("Lrn010").WithPathwayCode(999).Build(),
                new LearnerBuilder().WithLearnRefNumber("Lrn011").WithPathwayCode(null).Build(),
                new LearnerBuilder().WithLearnRefNumber("Lrn012").WithNegotiatedPrice(999).Build(),
                new LearnerBuilder().WithLearnRefNumber("Lrn013").WithNegotiatedPrice(null).Build(),
                new LearnerBuilder().WithLearnRefNumber("Lrn014").WithLearnStartDate(new DateTime(2016, 8, 31)).Build(),
                new LearnerBuilder().WithLearnRefNumber("Lrn015").WithLearnStartDate(null).Build()
            };

            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = commitments,
                Learners = learners
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(14, response.ValidationErrors.Length);
            Assert.AreEqual(1, response.LearnerCommitments.Count(l =>
                                                                    l.CommitmentId == commitments[0].CommitmentId &&
                                                                    l.Ukprn == learners[0].Ukprn &&
                                                                    l.LearnerReferenceNumber == learners[0].LearnerReferenceNumber &&
                                                                    learners[0].AimSequenceNumber.HasValue &&
                                                                    l.AimSequenceNumber == learners[0].AimSequenceNumber.Value));
        }
    }
}

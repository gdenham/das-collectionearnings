using System;
using System.Linq;
using System.Runtime.InteropServices;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.RunDataLockValidationQuery.RunDataLockValidationQueryHandler
{
    public class WhenHandling
    {
        private static readonly object[] LearnersWithMismatchingUln =
        {
            new object[] {new LearnerEntityBuilder().WithUln(1000000018).Build()},
            new object[] {new LearnerEntityBuilder().WithUln(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingFramework =
        {
            new object[] {new LearnerEntityBuilder().WithFrameworkCode(999).Build()},
            new object[] {new LearnerEntityBuilder().WithFrameworkCode(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingProgramme =
        {
            new object[] {new LearnerEntityBuilder().WithProgrammeType(999).Build()},
            new object[] {new LearnerEntityBuilder().WithProgrammeType(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingPathway =
        {
            new object[] {new LearnerEntityBuilder().WithPathwayCode(999).Build()},
            new object[] {new LearnerEntityBuilder().WithPathwayCode(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingPrice =
        {
            new object[] {new LearnerEntityBuilder().WithNegotiatedPrice(999).Build()},
            new object[] {new LearnerEntityBuilder().WithNegotiatedPrice(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingStartDates =
       {
            new object[] {new LearnerEntityBuilder().WithLearnStartDate(new DateTime(2016, 8, 31)).Build()},
            new object[] {new LearnerEntityBuilder().WithLearnStartDate(null).Build()}
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
                    new LearnerEntityBuilder().Build()
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
                    new LearnerEntityBuilder().WithUkprn(10007458).Build()
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
        public void ThenErrorExpectedForNoUlnMatch(Infrastructure.Data.Entities.LearnerEntity dasLearner)
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
                    new LearnerEntityBuilder().WithStandardCode(998).Build()
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
        public void ThenErrorExpectedForNoFrameworkMatch(Infrastructure.Data.Entities.LearnerEntity dasLearner)
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
        public void ThenErrorExpectedForNoProgrammeMatch(Infrastructure.Data.Entities.LearnerEntity dasLearner)
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
        public void ThenErrorExpectedForNoPathwayMatch(Infrastructure.Data.Entities.LearnerEntity dasLearner)
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
        public void ThenErrorExpectedForNoPriceMatch(Infrastructure.Data.Entities.LearnerEntity dasLearner)
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
        public void ThenErrorExpectedForNoStartDateOrEarlierStartDate(Infrastructure.Data.Entities.LearnerEntity dasLearner)
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
                    new LearnerEntityBuilder().Build()
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.MultipleMatches));
            Assert.AreEqual(1, response.LearnerCommitments.Count(l => l.CommitmentId == commitments[0].CommitmentId));
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
                new LearnerEntityBuilder().Build(),
                new LearnerEntityBuilder().WithLearnRefNumber("Lrn002").WithUkprn(10007458).Build(),
                new LearnerEntityBuilder().WithLearnRefNumber("Lrn003").WithUln(1000000018).Build(),
                new LearnerEntityBuilder().WithLearnRefNumber("Lrn004").WithUln(null).Build(),
                new LearnerEntityBuilder().WithLearnRefNumber("Lrn005").WithStandardCode(998).Build(),
                new LearnerEntityBuilder().WithLearnRefNumber("Lrn006").WithFrameworkCode(999).Build(),
                new LearnerEntityBuilder().WithLearnRefNumber("Lrn007").WithFrameworkCode(null).Build(),
                new LearnerEntityBuilder().WithLearnRefNumber("Lrn008").WithProgrammeType(999).Build(),
                new LearnerEntityBuilder().WithLearnRefNumber("Lrn009").WithProgrammeType(null).Build(),
                new LearnerEntityBuilder().WithLearnRefNumber("Lrn010").WithPathwayCode(999).Build(),
                new LearnerEntityBuilder().WithLearnRefNumber("Lrn011").WithPathwayCode(null).Build(),
                new LearnerEntityBuilder().WithLearnRefNumber("Lrn012").WithNegotiatedPrice(999).Build(),
                new LearnerEntityBuilder().WithLearnRefNumber("Lrn013").WithNegotiatedPrice(null).Build(),
                new LearnerEntityBuilder().WithLearnRefNumber("Lrn014").WithLearnStartDate(new DateTime(2016, 8, 31)).Build(),
                new LearnerEntityBuilder().WithLearnRefNumber("Lrn015").WithLearnStartDate(null).Build()
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
                                                                    l.LearnerReferenceNumber == learners[0].LearnRefNumber &&
                                                                    learners[0].AimSeqNumber.HasValue &&
                                                                    l.AimSequenceNumber == learners[0].AimSeqNumber.Value));
        }

        [Test]
        public void Bla()
        {
            //// Arrange
            //_request = new RunDataLockValidationQueryRequest
            //{
            //    Commitments = new[]
            //    {
            //        new CommitmentBuilder().WithCommitmentId("ABCDEFG").WithUln(1000000018).Build(),
            //        new CommitmentBuilder().Build()
            //    },
            //    Learners = new[]
            //    {
            //        new LearnerEntityBuilder().Build()
            //    }
            //};

            //// Act
            //var response = _handler.Handle(_request);

            //// Assert
            //Assert.IsNotNull(response);
            //Assert.IsTrue(response.IsValid);
            //Assert.AreEqual(0, response.ValidationErrors.Length);
        }
    }
}

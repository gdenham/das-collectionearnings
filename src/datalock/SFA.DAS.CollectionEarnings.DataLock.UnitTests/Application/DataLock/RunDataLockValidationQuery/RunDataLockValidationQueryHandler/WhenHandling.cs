using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Enums;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DataLock.RunDataLockValidationQuery.RunDataLockValidationQueryHandler
{
    public class WhenHandling
    {
        private static readonly object[] PriceEpisodesWithMismatchingUln =
        {
            new object[] {new PriceEpisodeBuilder().WithUln(1000000018).Build()},
            new object[] {new PriceEpisodeBuilder().WithUln(null).Build()}
        };

        private static readonly object[] PriceEpisodesWithMismatchingFramework =
        {
            new object[] {new PriceEpisodeBuilder().WithFrameworkCode(999).Build()},
            new object[] {new PriceEpisodeBuilder().WithFrameworkCode(null).Build()}
        };

        private static readonly object[] PriceEpisodesWithMismatchingProgramme =
        {
            new object[] {new PriceEpisodeBuilder().WithProgrammeType(999).Build()},
            new object[] {new PriceEpisodeBuilder().WithProgrammeType(null).Build()}
        };

        private static readonly object[] PriceEpisodesWithMismatchingPathway =
        {
            new object[] {new PriceEpisodeBuilder().WithPathwayCode(999).Build()},
            new object[] {new PriceEpisodeBuilder().WithPathwayCode(null).Build()}
        };

        private static readonly object[] PriceEpisodesWithMismatchingPrice =
        {
            new object[] {new PriceEpisodeBuilder().WithNegotiatedPrice(999).Build()},
            new object[] {new PriceEpisodeBuilder().WithNegotiatedPrice(null).Build()}
        };

        private static readonly object[] PriceEpisodesWithMismatchingStartDates =
       {
            new object[] {new PriceEpisodeBuilder().WithLearnStartDate(new DateTime(2016, 8, 31)).Build()}
        };

        private RunDataLockValidationQueryRequest _request;
        private readonly CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery.RunDataLockValidationQueryHandler _handler = new CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery.RunDataLockValidationQueryHandler();

        [Test]
        public void ThenNoErrorExpectedForMatchingCommitmentAndPriceEpisodeData()
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                PriceEpisodes = new[]
                {
                    new PriceEpisodeBuilder().Build()
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
                PriceEpisodes = new[]
                {
                    new PriceEpisodeBuilder().WithUkprn(10007458).Build()
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
        [TestCaseSource(nameof(PriceEpisodesWithMismatchingUln))]
        public void ThenErrorExpectedForNoUlnMatch(CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode dasLearner)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                PriceEpisodes = new[]
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
                PriceEpisodes = new[]
                {
                    new PriceEpisodeBuilder().WithStandardCode(998).Build()
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
        [TestCaseSource(nameof(PriceEpisodesWithMismatchingFramework))]
        public void ThenErrorExpectedForNoFrameworkMatch(CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode dasLearner)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                PriceEpisodes = new[]
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
        [TestCaseSource(nameof(PriceEpisodesWithMismatchingProgramme))]
        public void ThenErrorExpectedForNoProgrammeMatch(CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode dasLearner)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                PriceEpisodes = new[]
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
        [TestCaseSource(nameof(PriceEpisodesWithMismatchingPathway))]
        public void ThenErrorExpectedForNoPathwayMatch(CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode dasLearner)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                PriceEpisodes = new[]
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
        [TestCaseSource(nameof(PriceEpisodesWithMismatchingPrice))]
        public void ThenErrorExpectedForNoPriceMatch(CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode dasLearner)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                PriceEpisodes = new[]
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
        [TestCaseSource(nameof(PriceEpisodesWithMismatchingStartDates))]
        public void ThenErrorExpectedForNoStartDateOrEarlierStartDate(CollectionEarnings.DataLock.Application.PriceEpisode.PriceEpisode dasLearner)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().Build()
                },
                PriceEpisodes = new[]
                {
                    dasLearner
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.EarlierStartDate));
            Assert.AreEqual(0, response.LearnerCommitments.Length);
        }

        [Test]
        public void ThenErrorExpectedForMultiplePriceMatchingCommitments()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentBuilder().Build(),
                new CommitmentBuilder().WithCommitmentId(2).Build()
            };

            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = commitments,
                PriceEpisodes = new[]
                {
                    new PriceEpisodeBuilder().Build()
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
        public void ThenMultipleErrorsExpectedForMultiplePriceEpisodesProvided()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentBuilder().Build()
            };

            var priceEpisodes = new[]
            {
                new PriceEpisodeBuilder().Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn002").WithUkprn(10007458).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn003").WithUln(1000000018).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn004").WithUln(null).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn005").WithStandardCode(998).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn006").WithFrameworkCode(999).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn007").WithFrameworkCode(null).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn008").WithProgrammeType(999).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn009").WithProgrammeType(null).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn010").WithPathwayCode(999).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn011").WithPathwayCode(null).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn012").WithNegotiatedPrice(999).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn013").WithNegotiatedPrice(null).Build(),
                new PriceEpisodeBuilder().WithLearnRefNumber("Lrn014").WithLearnStartDate(new DateTime(2016, 8, 31)).Build()
            };

            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = commitments,
                PriceEpisodes = priceEpisodes
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(13, response.ValidationErrors.Length);
            Assert.AreEqual(1, response.LearnerCommitments.Count(l =>
                                                                    l.CommitmentId == commitments[0].CommitmentId &&
                                                                    l.VersionId == commitments[0].VersionId &&
                                                                    l.Ukprn == priceEpisodes[0].Ukprn &&
                                                                    l.LearnerReferenceNumber == priceEpisodes[0].LearnerReferenceNumber &&
                                                                    priceEpisodes[0].AimSequenceNumber.HasValue &&
                                                                    l.AimSequenceNumber == priceEpisodes[0].AimSequenceNumber.Value));
        }

        [Test]
        [TestCase(PaymentStatus.PendingApproval)]
        [TestCase(PaymentStatus.Paused)]
        [TestCase(PaymentStatus.Cancelled)]
        [TestCase(PaymentStatus.Deleted)]
        public void ThenErrorExpectedForNoPayableCommitments(PaymentStatus notPayablePaymentStatus)
        {
            // Arrange
            _request = new RunDataLockValidationQueryRequest
            {
                Commitments = new[]
                {
                    new CommitmentBuilder().WithPaymentStatus(notPayablePaymentStatus).Build()
                },
                PriceEpisodes = new[]
                {
                    new PriceEpisodeBuilder().Build()
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.ValidationErrors.Count(ve => ve.RuleId == DataLockErrorCodes.NotPayable));
            Assert.AreEqual(0, response.LearnerCommitments.Length);
        }
    }
}

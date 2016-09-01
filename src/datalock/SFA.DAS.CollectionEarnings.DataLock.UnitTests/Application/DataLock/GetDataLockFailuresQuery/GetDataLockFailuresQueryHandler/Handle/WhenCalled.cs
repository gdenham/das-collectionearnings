using System.Linq;
using NUnit.Framework;
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
            new object[] {new DasLearnerBuilder().WithFworkCode(999).Build()},
            new object[] {new DasLearnerBuilder().WithFworkCode(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingProgramme =
        {
            new object[] {new DasLearnerBuilder().WithProgType(999).Build()},
            new object[] {new DasLearnerBuilder().WithProgType(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingPathway =
        {
            new object[] {new DasLearnerBuilder().WithPwayCode(999).Build()},
            new object[] {new DasLearnerBuilder().WithPwayCode(null).Build()}
        };

        private static readonly object[] LearnersWithMismatchingPrice =
        {
            new object[] {new DasLearnerBuilder().WithTbFinAmount(999).Build()},
            new object[] {new DasLearnerBuilder().WithTbFinAmount(null).Build()}
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
            Assert.AreEqual(1, response.Items.Count(ve => ve.RuleId == "DLOCK_01"));
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
            Assert.AreEqual(1, response.Items.Count(ve => ve.RuleId == "DLOCK_02"));
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
                    new DasLearnerBuilder().WithStdCode(998).Build()
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(1, response.Items.Count(ve => ve.RuleId == "DLOCK_03"));
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
            Assert.AreEqual(1, response.Items.Count(ve => ve.RuleId == "DLOCK_04"));
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
            Assert.AreEqual(1, response.Items.Count(ve => ve.RuleId == "DLOCK_05"));
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
            Assert.AreEqual(1, response.Items.Count(ve => ve.RuleId == "DLOCK_06"));
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
            Assert.AreEqual(1, response.Items.Count(ve => ve.RuleId == "DLOCK_07"));
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
            Assert.AreEqual(1, response.Items.Count(ve => ve.RuleId == "DLOCK_08"));
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
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn005").WithStdCode(998).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn006").WithFworkCode(999).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn007").WithFworkCode(null).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn008").WithProgType(999).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn009").WithProgType(null).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn010").WithPwayCode(999).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn011").WithPwayCode(null).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn012").WithTbFinAmount(999).Build(),
                    new DasLearnerBuilder().WithLearnRefNumber("Lrn013").WithTbFinAmount(null).Build()
                }
            };

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(12, response.Items.Count());
        }
    }
}

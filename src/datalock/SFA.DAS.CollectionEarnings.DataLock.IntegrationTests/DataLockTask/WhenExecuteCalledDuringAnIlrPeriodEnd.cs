using System;
using System.Collections.Generic;
using System.Linq;
using CS.Common.External.Interfaces;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.DataLockTask
{
    public class WhenExecuteCalledDuringAnIlrPeriodEnd
    {
        private readonly string _transientConnectionString = GlobalTestContext.Instance.PeriodEndConnectionString;

        private IExternalTask _task;
        private IExternalContext _context;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
            TestDataHelper.PeriodEndAddCollectionPeriod();

            _task = new DataLock.DataLockTask();

            _context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, _transientConnectionString},
                    {ContextPropertyKeys.LogLevel, "Trace"}
                }
            };
        }

        private void SetupCommitmentData(CommitmentEntity commitment)
        {
            TestDataHelper.PeriodEndAddCommitment(commitment);
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingUkprn()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndUkprnMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().Withukprn(999).Build());
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingUkprn && e.PriceEpisodeIdentifier == "27-25-2016-07-17"));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingUln()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndUlnMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithUln(999).Build());
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingUln));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingStandard()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndStandardMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(999).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build());
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingStandard));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingFramework()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndFrameworkMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(null).WithFrameworkCode(999).Build());
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingFramework));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingProgramme()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndProgrammeMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(null).WithProgrammeType(999).Build());
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingProgramme));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingPathway()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndPathwayMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(null).WithPathwayCode(999).Build());
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingPathway));
        }

        [Test]
        public void ThenValidationErrorsAddedForMismatchingPrice()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndPriceMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithUln(1000000000).WithStandardCode(999).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build());
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(2, errors.Length);
            Assert.AreEqual(2, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingPrice));
        }

        [Test]
        public void ThenValidationErrorsAddedForIlrEarlierStartDate()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndEarlierStartDateMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithUln(1000000000).WithStandardCode(999).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build());
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(2, errors.Length);
            Assert.AreEqual(2, errors.Count(e => e.RuleId == DataLockErrorCodes.EarlierStartDate));
        }

        [Test]
        public void ThenValidationErrorsAddedForMultipleMatchingCommitments()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndMultipleMatches.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithUln(1000000000).WithStandardCode(999).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithUln(1000000000).WithCommitmentId(3).WithStandardCode(999).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(4).WithUln(1000000027).WithStandardCode(null).Build());
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(2, errors.Length);
            Assert.AreEqual(2, errors.Count(e => e.RuleId == DataLockErrorCodes.MultipleMatches));
        }

        [Test]
        public void ThenMatchingLearnerAndCommitmentAddedForMatchFound()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentEntityBuilder().WithUln(1000000000).WithStandardCode(999).Build(),
                new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build()
            };

            TestDataHelper.PeriodEndExecuteScript("PeriodEndMatchFound.sql");
            SetupCommitmentData(commitments[0]);
            SetupCommitmentData(commitments[1]);
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var learnerAndCommitmentMatches = TestDataHelper.PeriodEndGetLearnerAndCommitmentMatches();
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Length);
            Assert.IsNotNull(learnerAndCommitmentMatches);
            Assert.AreEqual(2, learnerAndCommitmentMatches.Length);
            Assert.AreEqual(1, learnerAndCommitmentMatches.Count(l => l.CommitmentId == commitments[0].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2016-09-17"));
            Assert.AreEqual(1, learnerAndCommitmentMatches.Count(l => l.CommitmentId == commitments[1].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2016-09-19"));
        }

        [Test]
        public void ThenForAChangeOfEmployersMatchingLearnerAndCommitmentsAreAddedWhenMatchesAreFound()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentEntityBuilder().WithStandardCode(27).WithStartDate(new DateTime(2017, 8, 1)).WithEndDate(new DateTime(2017, 10, 31)).Build(),
                new CommitmentEntityBuilder().WithCommitmentId(2).WithStandardCode(27).WithStartDate(new DateTime(2017, 11, 1)).WithEndDate(new DateTime(2018, 8, 31)).WithAgreedCost(5625m).Build()
            };

            TestDataHelper.PeriodEndExecuteScript("PeriodEndLearnerChangesEmployers.sql");
            SetupCommitmentData(commitments[0]);
            SetupCommitmentData(commitments[1]);
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var learnerAndCommitmentMatches = TestDataHelper.PeriodEndGetLearnerAndCommitmentMatches();
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Length);
            Assert.IsNotNull(learnerAndCommitmentMatches);
            Assert.AreEqual(2, learnerAndCommitmentMatches.Length);
            Assert.AreEqual(1, learnerAndCommitmentMatches.Count(l => l.CommitmentId == commitments[0].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2017-08-01"));
            Assert.AreEqual(1, learnerAndCommitmentMatches.Count(l => l.CommitmentId == commitments[1].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2017-11-01"));
        }
    }
}
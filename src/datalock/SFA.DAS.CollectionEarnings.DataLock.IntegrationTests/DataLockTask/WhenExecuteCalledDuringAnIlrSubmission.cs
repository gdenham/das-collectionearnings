using System.Collections.Generic;
using System.Linq;
using CS.Common.External.Interfaces;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools.Ilr;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.DataLockTask
{
    public class WhenExecuteCalledDuringAnIlrSubmission
    {
        private readonly string _transientConnectionString = GlobalTestContext.Instance.SubmissionConnectionString;

        private IExternalTask _task;
        private IExternalContext _context;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

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

        private void SetupIlrData(string ilrFile)
        {
            var shredder = new Shredder(GlobalTestContext.Instance.SubmissionConnectionString, ilrFile);
            shredder.Shred();
        }

        private void SetupCommitmentData(CommitmentEntity commitment)
        {
            TestDataHelper.AddCommitment(commitment);
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingUkprn()
        {
            // Arrange
            SetupIlrData(@"\Tools\Ilr\Files\IlrUkprnMismatch.xml");
            SetupCommitmentData(new CommitmentEntityBuilder().Build());
            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingUkprn));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingUln()
        {
            // Arrange
            SetupIlrData(@"\Tools\Ilr\Files\IlrUlnMismatch.xml");
            SetupCommitmentData(new CommitmentEntityBuilder().Build());
            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingUln));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingStandard()
        {
            // Arrange
            SetupIlrData(@"\Tools\Ilr\Files\IlrStandardMismatch.xml");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(999).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build());
            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingStandard));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingFramework()
        {
            // Arrange
            SetupIlrData(@"\Tools\Ilr\Files\IlrFrameworkMismatch.xml");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(null).WithFrameworkCode(999).Build());
            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingFramework));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingProgramme()
        {
            // Arrange
            SetupIlrData(@"\Tools\Ilr\Files\IlrProgrammeMismatch.xml");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(null).WithProgrammeType(999).Build());
            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingProgramme));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingPathway()
        {
            // Arrange
            SetupIlrData(@"\Tools\Ilr\Files\IlrPathwayMismatch.xml");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(null).WithPathwayCode(999).Build());
            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingPathway));
        }

        [Test]
        public void ThenValidationErrorsAddedForMismatchingPrice()
        {
            // Arrange
            SetupIlrData(@"\Tools\Ilr\Files\IlrPriceMismatch.xml");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(999).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build());
            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(2, errors.Length);
            Assert.AreEqual(2, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingPrice));
        }

        [Test]
        public void ThenValidationErrorsAddedForIlrEarlierStartMonth()
        {
            // Arrange
            SetupIlrData(@"\Tools\Ilr\Files\IlrEarlierStartMonth.xml");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(999).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build());
            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(2, errors.Length);
            Assert.AreEqual(2, errors.Count(e => e.RuleId == DataLockErrorCodes.EarlierStartMonth));
        }

        [Test]
        public void ThenValidationErrorsAddedForMultipleMatchingCommitments()
        {
            // Arrange
            SetupIlrData(@"\Tools\Ilr\Files\IlrMultipleMatchingCommitments.xml");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(999).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(3).WithStandardCode(999).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(4).WithUln(1000000027).WithStandardCode(null).Build());
            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(2, errors.Length);
            Assert.AreEqual(2, errors.Count(e => e.RuleId == DataLockErrorCodes.MultipleMatches));
        }

        [TestCase("IlrLearnerAndCommitmentMatch.xml")]
        [TestCase("IlrTnp1AndTnp2.xml")]
        public void ThenMatchingLearnerAndCommitmentAddedForMatchFound(string ilrFile)
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentEntityBuilder().WithStandardCode(999).Build(),
                new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build()
            };

            SetupIlrData(@"\Tools\Ilr\Files\" + ilrFile);
            SetupCommitmentData(commitments[0]);
            SetupCommitmentData(commitments[1]);
            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var learnerAndCommitmentMatches = TestDataHelper.GetLearnerAndCommitmentMatches();
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Length);
            Assert.IsNotNull(learnerAndCommitmentMatches);
            Assert.AreEqual(2, learnerAndCommitmentMatches.Length);
            Assert.AreEqual(1, learnerAndCommitmentMatches.Count(l => l.CommitmentId == commitments[0].CommitmentId));
            Assert.AreEqual(1, learnerAndCommitmentMatches.Count(l => l.CommitmentId == commitments[1].CommitmentId));
        }
    }
}

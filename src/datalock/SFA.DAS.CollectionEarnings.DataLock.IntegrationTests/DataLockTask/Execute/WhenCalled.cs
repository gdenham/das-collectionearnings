using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CS.Common.External.Interfaces;
using Dapper;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Context;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools.Ilr;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.DataLockTask.Execute
{
    public class WhenCalled
    {
        private readonly string _transientConnectionString = ConnectionStringFactory.GetTransientConnectionString();

        private IExternalTask _task;
        private IExternalContext _context;

        [SetUp]
        public void Arrange()
        {
            SetUpIlrDatabase();

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

        private void SetUpIlrDatabase()
        {
            Database.Clean(_transientConnectionString);

            // ILR data
            var shredder = new Shredder();
            shredder.Shred();

            // Commitment data
            // Commitment for learner with uln 1000000019 - will pass double lock because all matches
            Database.AddCommitment(_transientConnectionString, new CommitmentBuilder().WithAgreedCost(2750).Build());

            // Commitment for learner with uln 1000000027 - will fail double lock for not matching the agreed cost
            Database.AddCommitment(_transientConnectionString, new CommitmentBuilder().WithCommitmentId("C-002").WithUln(1000000027).Build());

            // Commitment for learner with uln 1000000035 - will fail double lock for not matching the framework code
            Database.AddCommitment(_transientConnectionString, new CommitmentBuilder().WithCommitmentId("C-003").WithUln(1000000035).Build());
        }

        [Test]
        public void ThenValidationErrorAddedSuccessfully()
        {
            // Act
            _task.Execute(_context);

            // Assert - expecting 8 validation errors: 2 for existing commitments with mismatching data and 6 for not having any commitment data
            using (var connection = new SqlConnection(_transientConnectionString))
            {
                var errors = connection.Query<ValidationError>(ValidationError.SelectAll).ToList();

                Assert.IsNotNull(errors);
                Assert.AreEqual(8, errors.Count);
                Assert.AreEqual(6, errors.Count(e => e.RuleId == "DLOCK_02"));
                Assert.AreEqual(1, errors.Count(e => e.RuleId == "DLOCK_04"));
                Assert.AreEqual(1, errors.Count(e => e.RuleId == "DLOCK_07"));
            }
        }

    }
}

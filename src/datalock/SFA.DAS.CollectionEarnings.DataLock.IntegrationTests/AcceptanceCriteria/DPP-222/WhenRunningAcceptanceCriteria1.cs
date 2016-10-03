using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CS.Common.External.Interfaces;
using Dapper;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools.Ilr;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.AcceptanceCriteria
{
    public class WhenRunningAcceptanceCriteria1
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
            var shredder = new Shredder(@"\Tools\Ilr\Files\DPP-222\IlrAcceptanceCriteria1.xml");
            shredder.Shred();

            // Commitment data
            Database.AddCommitment(
                _transientConnectionString,
                new CommitmentBuilder()
                    .WithUln(1000000000)
                    .WithStartDate(new DateTime(2017, 9, 1))
                    .WithStandardCode(999)
                    .WithProgrammeType(null)
                    .WithFrameworkCode(null)
                    .WithPathwayCode(null)
                    .WithAgreedCost(3000)
                    .Build());
        }

        [Test]
        public void ThenValidationErrorAddedSuccessfully()
        {
            // Act
            _task.Execute(_context);

            // Assert
            using (var connection = new SqlConnection(_transientConnectionString))
            {
                var errors = connection.Query<ValidationErrorEntity>(ValidationErrorEntity.SelectAll).ToList();

                Assert.IsNotNull(errors);
                Assert.AreEqual(1, errors.Count);
                Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.EarlierStartMonth));
            }
        }
    }
}
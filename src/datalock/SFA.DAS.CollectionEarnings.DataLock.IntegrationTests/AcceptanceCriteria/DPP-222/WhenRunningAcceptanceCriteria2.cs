using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CS.Common.External.Interfaces;
using Dapper;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Context;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools.Ilr;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.AcceptanceCriteria
{
    public class WhenRunningAcceptanceCriteria2
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
            var shredder = new Shredder(@"\Tools\Ilr\Files\DPP-222\IlrAcceptanceCriteria2.xml");
            shredder.Shred();

            // Commitment data
            Database.AddCommitment(
                _transientConnectionString,
                new CommitmentBuilder()
                    .WithUln(1000000000)
                    .WithStartDate(new DateTime(2017, 9, 1))
                    .WithStandardCode(null)
                    .WithProgrammeType(20)
                    .WithFrameworkCode(550)
                    .WithPathwayCode(6)
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
                var errors = connection.Query<ValidationError>(ValidationError.SelectAll).ToList();

                Assert.IsNotNull(errors);
                Assert.AreEqual(1, errors.Count);
                Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.EarlierStartMonth));
            }
        }
    }
}
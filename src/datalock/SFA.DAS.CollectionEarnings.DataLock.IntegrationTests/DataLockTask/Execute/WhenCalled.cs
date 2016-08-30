using System.Collections.Generic;
using CS.Common.External.Interfaces;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Common.Tests.Data;
using SFA.DAS.CollectionEarnings.DataLock.Common.Tests.ExternalContext;
using SFA.DAS.CollectionEarnings.DataLock.Context;

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
            Database.Clean(_transientConnectionString);

            _context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, _transientConnectionString},
                    {ContextPropertyKeys.LogLevel, "Info"}
                }
            };
        }

        [Test]
        public void ThenValidationErrorAddedSuccessfully()
        {
            // Arrange
            _task = new DataLock.DataLockTask();

            // Act
            _task.Execute(_context);

            // Assert
            //using (var connection = new SqlConnection(_transientConnectionString))
            //{
            //    var errors = connection.Query(ValidationError.SelectAll);

            //    Assert.IsNotNull(errors);
            //    Assert.AreEqual(1, errors.ToList().Count);
            //}
        }

    }
}

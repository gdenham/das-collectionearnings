﻿using System.Data.SqlClient;
using System.Linq;
using Dapper;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.ValidationErrorRepository
{
    public class WhenAddValidationErrorsCalled
    {
        private readonly string _transientConnectionString = ConnectionStringFactory.GetTransientConnectionString();

        private IValidationErrorRepository _validationErrorRepository;

        [SetUp]
        public void Arrange()
        {
            Database.Clean(_transientConnectionString);

            _validationErrorRepository = new DataLock.Infrastructure.Data.Repositories.ValidationErrorRepository(_transientConnectionString);
        }

        [Test]
        public void ThenValidationErrorsAddedSuccessfully()
        {
            // Arrange
            var validationErrors = new[]
            {
                new ValidationErrorBuilder().Build(),
                new ValidationErrorBuilder().WithAimSeqNumber(2).WithRuleId(DataLockErrorCodes.MismatchingUln).Build(),

                new ValidationErrorBuilder().WithLearnRefNumber(string.Empty).Build(),
                new ValidationErrorBuilder().WithLearnRefNumber(null).Build(),

                new ValidationErrorBuilder().WithAimSeqNumber(null).Build(),

                new ValidationErrorBuilder().WithRuleId(string.Empty).Build(),
                new ValidationErrorBuilder().WithRuleId(null).Build()
            };

            // Act
            _validationErrorRepository.AddValidationErrors(validationErrors);

            // Assert
            using (var connection = new SqlConnection(_transientConnectionString))
            {
                var errors = connection.Query(ValidationErrorEntity.SelectAll);

                Assert.IsNotNull(errors);
                Assert.AreEqual(7, errors.ToList().Count);
            }
        }
    }
}

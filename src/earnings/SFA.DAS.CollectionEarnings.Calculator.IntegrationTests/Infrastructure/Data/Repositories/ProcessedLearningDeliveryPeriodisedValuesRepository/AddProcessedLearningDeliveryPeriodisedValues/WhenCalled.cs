using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Infrastructure.Data.Repositories.ProcessedLearningDeliveryPeriodisedValuesRepository.AddProcessedLearningDeliveryPeriodisedValues
{
    public class WhenCalled
    {
        private static readonly object[] InvalidProcessedLearningDeliveryPeriodisedValuesWithExpectedExceptionTypes =
        {
            new object[] {
                // Duplicate PK
                new[]
                {
                    new ProcessedLearningDeliveryPeriodisedValuesBuilder().Build(),
                    new ProcessedLearningDeliveryPeriodisedValuesBuilder().Build()
                },
                typeof(SqlException)
            },
            new object[]
            {
                // Out of bounds Period_1 -> Period_12 values
                new[]
                {
                    new ProcessedLearningDeliveryPeriodisedValuesBuilder().WithPeriodValue(12345678901.00m).Build()
                },
                typeof(InvalidOperationException)
            }
        };

        private readonly string _transientConnectionString = GlobalTestContext.Instance.ConnectionString;

        private IProcessedLearningDeliveryPeriodisedValuesRepository _repository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _repository = new Calculator.Infrastructure.Data.Repositories.ProcessedLearningDeliveryPeriodisedValuesRepository(_transientConnectionString);
        }

        [Test]
        public void ThenExpectingDatabaseEntriesForAddProcessedLearningDeliveryPeriodisedValuesWithValidInput()
        {
            // Arrange
            var periodisedValues = new[]
            {
                new ProcessedLearningDeliveryPeriodisedValuesBuilder().Build(),
                new ProcessedLearningDeliveryPeriodisedValuesBuilder().WithAimSeqNumber(2).Build(),
                new ProcessedLearningDeliveryPeriodisedValuesBuilder().WithAimSeqNumber(3).Build(),
                new ProcessedLearningDeliveryPeriodisedValuesBuilder().WithAimSeqNumber(4).Build(),
                new ProcessedLearningDeliveryPeriodisedValuesBuilder().WithAimSeqNumber(5).Build()
            };

            // Act
            _repository.AddProcessedLearningDeliveryPeriodisedValues(periodisedValues);

            // Assert
            using (var connection = new SqlConnection(_transientConnectionString))
            {
                var rows = connection.Query<ProcessedLearningDeliveryPeriodisedValues>("SELECT * FROM [Rulebase].[AE_LearningDelivery_PeriodisedValues]");

                Assert.IsNotNull(rows);
                Assert.AreEqual(5, rows.Count());
            }
        }

        [Test]
        [TestCaseSource(nameof(InvalidProcessedLearningDeliveryPeriodisedValuesWithExpectedExceptionTypes))]
        public void ThenExpectingExceptionForAddProcessedLearningDeliveryPeriodisedValuesWithInvalidInput(ProcessedLearningDeliveryPeriodisedValues[] periodisedValues, Type exceptionType)
        {
            // Assert
            Assert.Throws(exceptionType, () => _repository.AddProcessedLearningDeliveryPeriodisedValues(periodisedValues));
        }
    }
}
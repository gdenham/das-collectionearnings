using System;
using System.Data.SqlClient;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Infrastructure.Data.Repositories.ProcessedLearningDeliveryPeriodRepository.AddProcessedLearningDeliveryPeriod
{
    public class WhenCalled
    {
        private static readonly object[] InvalidProcessedLearningDeliveryPeriodWithExpectedExceptionTypes =
        {
            new object[] {
                // Duplicate PK
                new[]
                {
                    new ProcessedLearningDeliveryPeriodBuilder().Build(),
                    new ProcessedLearningDeliveryPeriodBuilder().Build()
                },
                typeof(SqlException)
            },
            new object[]
            {
                // Out of bounds values
                new[]
                {
                    new ProcessedLearningDeliveryPeriodBuilder().WithProgrammeAimBalPayment(12345678901.00m).WithProgrammeAimCompletionPayment(12345678901.00m).WithProgrammeAimOnProgPayment(12345678901.00m).Build()
                },
                typeof(InvalidOperationException)
            }
        };

        private readonly string _transientConnectionString = GlobalTestContext.Instance.ConnectionString;

        private IProcessedLearningDeliveryPeriodRepository _repository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _repository = new Calculator.Infrastructure.Data.Repositories.ProcessedLearningDeliveryPeriodRepository(_transientConnectionString);
        }

        [Test]
        public void ThenExpectingDatabaseEntriesForAddProcessedLearningDeliveryPeriodisedValuesWithValidInput()
        {
            // Arrange
            var periodValues = new[]
            {
                new ProcessedLearningDeliveryPeriodBuilder().Build(),
                new ProcessedLearningDeliveryPeriodBuilder().WithAimSeqNumber(2).Build(),
                new ProcessedLearningDeliveryPeriodBuilder().WithAimSeqNumber(3).Build(),
                new ProcessedLearningDeliveryPeriodBuilder().WithAimSeqNumber(4).Build(),
                new ProcessedLearningDeliveryPeriodBuilder().WithAimSeqNumber(5).Build()
            };

            // Act
            _repository.AddProcessedLearningDeliveryPeriod(periodValues);

            // Assert
            var rows = TestDataHelper.GetProcessedLearningDeliveryPeriods();

            Assert.IsNotNull(rows);
            Assert.AreEqual(5, rows.Length);
        }

        [Test]
        [TestCaseSource(nameof(InvalidProcessedLearningDeliveryPeriodWithExpectedExceptionTypes))]
        public void ThenExpectingExceptionForAddProcessedLearningDeliveryPeriodisedValuesWithInvalidInput(ProcessedLearningDeliveryPeriod[] periodValues, Type exceptionType)
        {
            // Assert
            Assert.Throws(exceptionType, () => _repository.AddProcessedLearningDeliveryPeriod(periodValues));
        }
    }
}
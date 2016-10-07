using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Data;
using SFA.DAS.CollectionEarnings.Calculator.Data.Entities;
using SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Data.Repositories.ProcessedLearningDeliveryRepository.AddProcessedLearningDeliveries
{
    public class WhenCalled
    {
        private static readonly object[] InvalidProcessedLearningDeliveriesWithExpectedExceptionTypes =
        {
            new object[] {
                // Duplicate PK
                new[]
                {
                    new ProcessedLearningDeliveryBuilder().Build(),
                    new ProcessedLearningDeliveryBuilder().Build()
                },
                typeof(SqlException)
            },
            new object[]
            {
                // Out of bounds MonthlyInstallment
                new[]
                {
                    new ProcessedLearningDeliveryBuilder().WithMonthlyInstallment(12345678901.00m).Build()
                },
                typeof(InvalidOperationException)
            },
            new object[]
            {
                // Out of bounds MonthlyInstallmentUncapped
                new[]
                {
                    new ProcessedLearningDeliveryBuilder().WithMonthlyInstallmentUncapped(12345678901.00m).Build()
                },
                typeof(InvalidOperationException)
            },
            new object[]
            {
                // Out of bounds CompletionPayment
                new[]
                {
                    new ProcessedLearningDeliveryBuilder().WithCompletionPayment(12345678901.00m).Build()
                },
                typeof(InvalidOperationException)
            },
            new object[]
            {
                // Out of bounds CompletionPaymentUncapped
                new[]
                {
                    new ProcessedLearningDeliveryBuilder().WithCompletionPaymentUncapped(12345678901.00m).Build()
                },
                typeof(InvalidOperationException)
            }
        };

        private readonly string _transientConnectionString = GlobalTestContext.Instance.ConnectionString;

        private IProcessedLearningDeliveryRepository _repository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _repository = new Calculator.Data.Repositories.ProcessedLearningDeliveryRepository(_transientConnectionString);
        }

        [Test]
        public void ThenExpectingDatabaseEntriesForAddProcessedLearningDeliveriesWithValidInput()
        {
            // Arrange
            var deliveries = new[]
            {
                new ProcessedLearningDeliveryBuilder().Build(),
                new ProcessedLearningDeliveryBuilder().WithAimSeqNumber(2).Build(),
                new ProcessedLearningDeliveryBuilder().WithAimSeqNumber(3).Build(),
                new ProcessedLearningDeliveryBuilder().WithAimSeqNumber(4).Build(),
                new ProcessedLearningDeliveryBuilder().WithAimSeqNumber(5).Build()
            };

            // Act
            _repository.AddProcessedLearningDeliveries(deliveries);

            // Assert
            using (var connection = new SqlConnection(_transientConnectionString))
            {
                var rows = connection.Query<ProcessedLearningDelivery>("SELECT * FROM [Rulebase].[AE_LearningDelivery]");

                Assert.IsNotNull(rows);
                Assert.AreEqual(5, rows.Count());
            }
        }

        [Test]
        [TestCaseSource(nameof(InvalidProcessedLearningDeliveriesWithExpectedExceptionTypes))]
        public void ThenExpectingExceptionForAddProcessedLearningDeliveriesWithInvalidInput(ProcessedLearningDelivery[] deliveries, Type exceptionType)
        {
            // Assert
            Assert.Throws(exceptionType, () => _repository.AddProcessedLearningDeliveries(deliveries));
        }
    }
}
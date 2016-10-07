using System.Collections.Generic;
using CS.Common.External.Interfaces;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Context;
using SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.ApprenticeshipEarningsTask.Execute
{
    public class WhenCalled
    {
        private readonly string _transientConnectionString = GlobalTestContext.Instance.ConnectionString;

        private IExternalTask _task;
        private IExternalContext _context;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _task = new Calculator.ApprenticeshipEarningsTask();

            _context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, _transientConnectionString},
                    {ContextPropertyKeys.LogLevel, "Trace"},
                    {EarningsContextPropertyKeys.YearOfCollection, "1718"}
                }
            };
        }

        [Test]
        public void ThenProcessedLearningDeliveryWithAssociatedPeriodisedValuesIsAdded()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcess.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var learningDeliveries = TestDataHelper.GetProcessedLearningDeliveries();
            var periodisedValues = TestDataHelper.GetProcessedLearningDeliveryPeriodisedValues();

            Assert.IsNotNull(learningDeliveries);
            Assert.IsNotNull(periodisedValues);

            Assert.AreEqual(1, learningDeliveries.Length);
            Assert.AreEqual(1, periodisedValues.Length);
        }

        [Test]
        public void ThenProcessedLearningDeliveryWithAssociatedPeriodisedValuesIsAddedForALearnerThatFinishedLate()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcessLateFinisher.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var learningDeliveries = TestDataHelper.GetProcessedLearningDeliveries();
            var periodisedValues = TestDataHelper.GetProcessedLearningDeliveryPeriodisedValues();

            Assert.IsNotNull(learningDeliveries);
            Assert.IsNotNull(periodisedValues);

            Assert.AreEqual(1, learningDeliveries.Length);
            Assert.AreEqual(1, periodisedValues.Length);

            Assert.AreEqual(160.00m, learningDeliveries[0].MonthlyInstallment);
            Assert.AreEqual(600.00m, learningDeliveries[0].CompletionPayment);

            Assert.AreEqual(160.00m, periodisedValues[0].Period_1);
            Assert.AreEqual(160.00m, periodisedValues[0].Period_2);
            Assert.AreEqual(0.00m, periodisedValues[0].Period_3);
            Assert.AreEqual(0.00m, periodisedValues[0].Period_4);
            Assert.AreEqual(600.00m, periodisedValues[0].Period_5);
            Assert.AreEqual(0.00m, periodisedValues[0].Period_6);
            Assert.AreEqual(0.00m, periodisedValues[0].Period_7);
            Assert.AreEqual(0.00m, periodisedValues[0].Period_8);
            Assert.AreEqual(0.00m, periodisedValues[0].Period_9);
            Assert.AreEqual(0.00m, periodisedValues[0].Period_10);
            Assert.AreEqual(0.00m, periodisedValues[0].Period_11);
            Assert.AreEqual(0.00m, periodisedValues[0].Period_12);
        }
    }
}
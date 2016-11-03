﻿using System.Collections.Generic;
using System.Linq;
using CS.Common.External.Interfaces;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDeliveryPeriodisedValues;
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
            Assert.AreEqual(1, periodisedValues.Count(pv => pv.AttributeName == AttributeNames.OnProgrammePayment));
            Assert.AreEqual(1, periodisedValues.Count(pv => pv.AttributeName == AttributeNames.CompletionPayment));
            Assert.AreEqual(1, periodisedValues.Count(pv => pv.AttributeName == AttributeNames.BalancingPayment));
        }

        [Test]
        public void ThenAProcessedLearningDeliveryIsAddedForALearnerThatFinishedLate()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcessLateFinisher.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var learningDeliveries = TestDataHelper.GetProcessedLearningDeliveries();

            Assert.IsNotNull(learningDeliveries);
        }

        [Test]
        public void ThenPeriodisedOnProgrammeValuesAreAddedForALearnerThatFinishedLate()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcessLateFinisher.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var periodisedValues = TestDataHelper.GetProcessedLearningDeliveryPeriodisedValues();

            Assert.IsNotNull(periodisedValues);

            var onProgrammeEarning = periodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.OnProgrammePayment);

            Assert.IsNotNull(onProgrammeEarning);

            Assert.AreEqual(160.00m, onProgrammeEarning.Period_1);
            Assert.AreEqual(160.00m, onProgrammeEarning.Period_2);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_3);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_4);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_5);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_6);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_7);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_8);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_9);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_10);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_11);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_12);
        }

        [Test]
        public void ThenPeriodisedCompletionValuesAreAddedForALearnerThatFinishedLate()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcessLateFinisher.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var periodisedValues = TestDataHelper.GetProcessedLearningDeliveryPeriodisedValues();

            Assert.IsNotNull(periodisedValues);

            var completionEarning = periodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.CompletionPayment);

            Assert.IsNotNull(completionEarning);

            Assert.AreEqual(0.00m, completionEarning.Period_1);
            Assert.AreEqual(0.00m, completionEarning.Period_2);
            Assert.AreEqual(0.00m, completionEarning.Period_3);
            Assert.AreEqual(0.00m, completionEarning.Period_4);
            Assert.AreEqual(600.00m, completionEarning.Period_5);
            Assert.AreEqual(0.00m, completionEarning.Period_6);
            Assert.AreEqual(0.00m, completionEarning.Period_7);
            Assert.AreEqual(0.00m, completionEarning.Period_8);
            Assert.AreEqual(0.00m, completionEarning.Period_9);
            Assert.AreEqual(0.00m, completionEarning.Period_10);
            Assert.AreEqual(0.00m, completionEarning.Period_11);
            Assert.AreEqual(0.00m, completionEarning.Period_12);
        }

        [Test]
        public void ThenPeriodisedBalancingWithZeroValuesAreAddedForALearnerThatFinishedLate()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcessLateFinisher.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var periodisedValues = TestDataHelper.GetProcessedLearningDeliveryPeriodisedValues();

            Assert.IsNotNull(periodisedValues);

            var balancingEarning = periodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.BalancingPayment);

            Assert.IsNotNull(balancingEarning);

            Assert.AreEqual(0.00m, balancingEarning.Period_1);
            Assert.AreEqual(0.00m, balancingEarning.Period_2);
            Assert.AreEqual(0.00m, balancingEarning.Period_3);
            Assert.AreEqual(0.00m, balancingEarning.Period_4);
            Assert.AreEqual(0.00m, balancingEarning.Period_5);
            Assert.AreEqual(0.00m, balancingEarning.Period_6);
            Assert.AreEqual(0.00m, balancingEarning.Period_7);
            Assert.AreEqual(0.00m, balancingEarning.Period_8);
            Assert.AreEqual(0.00m, balancingEarning.Period_9);
            Assert.AreEqual(0.00m, balancingEarning.Period_10);
            Assert.AreEqual(0.00m, balancingEarning.Period_11);
            Assert.AreEqual(0.00m, balancingEarning.Period_12);
        }

        [Test]
        public void ThenPeriodisedOnProgrammeValuesAreAddedForALearnerThatFinishedEarly()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcessEarlyFinisher.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var periodisedValues = TestDataHelper.GetProcessedLearningDeliveryPeriodisedValues();

            Assert.IsNotNull(periodisedValues);

            var onProgrammeEarning = periodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.OnProgrammePayment);

            Assert.IsNotNull(onProgrammeEarning);

            Assert.AreEqual(160.00m, onProgrammeEarning.Period_1);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_2);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_3);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_4);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_5);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_6);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_7);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_8);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_9);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_10);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_11);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_12);
        }

        [Test]
        public void ThenPeriodisedCompletionValuesAreAddedForALearnerThatFinishedEarly()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcessEarlyFinisher.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var periodisedValues = TestDataHelper.GetProcessedLearningDeliveryPeriodisedValues();

            Assert.IsNotNull(periodisedValues);

            var completionEarning = periodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.CompletionPayment);

            Assert.IsNotNull(completionEarning);

            Assert.AreEqual(600.00m, completionEarning.Period_1);
            Assert.AreEqual(0.00m, completionEarning.Period_2);
            Assert.AreEqual(0.00m, completionEarning.Period_3);
            Assert.AreEqual(0.00m, completionEarning.Period_4);
            Assert.AreEqual(0.00m, completionEarning.Period_5);
            Assert.AreEqual(0.00m, completionEarning.Period_6);
            Assert.AreEqual(0.00m, completionEarning.Period_7);
            Assert.AreEqual(0.00m, completionEarning.Period_8);
            Assert.AreEqual(0.00m, completionEarning.Period_9);
            Assert.AreEqual(0.00m, completionEarning.Period_10);
            Assert.AreEqual(0.00m, completionEarning.Period_11);
            Assert.AreEqual(0.00m, completionEarning.Period_12);
        }

        [Test]
        public void ThenPeriodisedBalancingValuesAreAddedForALearnerThatFinishedEarly()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcessEarlyFinisher.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var periodisedValues = TestDataHelper.GetProcessedLearningDeliveryPeriodisedValues();

            Assert.IsNotNull(periodisedValues);

            var balancingEarning = periodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.BalancingPayment);

            Assert.IsNotNull(balancingEarning);

            Assert.AreEqual(160.00m, balancingEarning.Period_1);
            Assert.AreEqual(0.00m, balancingEarning.Period_2);
            Assert.AreEqual(0.00m, balancingEarning.Period_3);
            Assert.AreEqual(0.00m, balancingEarning.Period_4);
            Assert.AreEqual(0.00m, balancingEarning.Period_5);
            Assert.AreEqual(0.00m, balancingEarning.Period_6);
            Assert.AreEqual(0.00m, balancingEarning.Period_7);
            Assert.AreEqual(0.00m, balancingEarning.Period_8);
            Assert.AreEqual(0.00m, balancingEarning.Period_9);
            Assert.AreEqual(0.00m, balancingEarning.Period_10);
            Assert.AreEqual(0.00m, balancingEarning.Period_11);
            Assert.AreEqual(0.00m, balancingEarning.Period_12);
        }
    }
}
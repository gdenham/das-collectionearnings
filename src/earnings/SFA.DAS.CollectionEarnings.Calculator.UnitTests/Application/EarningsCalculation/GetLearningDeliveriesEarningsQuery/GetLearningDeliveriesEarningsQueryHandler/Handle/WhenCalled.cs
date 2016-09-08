using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery;
using SFA.DAS.CollectionEarnings.Calculator.Tools.Providers;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery.GetLearningDeliveriesEarningsQueryHandler.Handle
{
    public class WhenCalled
    {
        #region Test Case Sources

        private static readonly object[] LearningDeliveriesToProcessAndExpectedMonthlyAndCompletionAmounts =
        {
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().Build()},
                1000.00m,
                3000.00m
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().WithLearnPlanEndDate(new DateTime(2018, 3, 15)).Build()},
                2000.00m,
                3000.00m
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 30)).Build()},
                923.0769230769230769230769231m,
                3000.00m
            }
        };

        private static readonly object[] LearningDeliveriesToProcessWithSubmissionDatesAndExpectedPaymentSchedules =
        {
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().Build()},
                new DateTime(2017, 9, 30),
                new[] {0.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m}
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().Build()},
                new DateTime(2018, 7, 15),
                new[] {0.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m}
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().WithLearnPlanEndDate(new DateTime(2018, 2, 28)).Build()},
                new DateTime(2018, 3, 15),
                new[] {0.00m, 2000.00m, 2000.00m, 2000.00m, 2000.00m, 2000.00m, 5000.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m}
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 30)).Build()},
                new DateTime(2018, 7, 15),
                new[] { 0.00m, 923.0769230769230769230769231m, 923.0769230769230769230769231m, 923.0769230769230769230769231m, 923.0769230769230769230769231m, 923.0769230769230769230769231m, 923.0769230769230769230769231m, 923.0769230769230769230769231m, 923.0769230769230769230769231m, 923.0769230769230769230769231m, 923.0769230769230769230769231m, 923.0769230769230769230769231m}
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().WithLearnStartDate(new DateTime(2017, 10, 1)).WithLearnPlanEndDate(new DateTime(2018, 3, 31)).WithNegotiatedPrice(3750).Build()},
                new DateTime(2018, 3, 15),
                new[] {0.00m, 0.00m, 500.00m, 500.00m, 500.00m, 500.00m, 500.00m, 1250.00m, 0.00m, 0.00m, 0.00m, 0.00m}
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().WithLearnStartDate(new DateTime(2016, 9, 1)).WithLearnPlanEndDate(new DateTime(2017, 9, 8)).Build()},
                new DateTime(2017, 9, 30),
                new[] {1000.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m}
            }
        };
        
        #endregion

        private Mock<IDateTimeProvider> _dateTimeProvider; 

        private GetLearningDeliveriesEarningsQueryRequest _request;
        private Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery.GetLearningDeliveriesEarningsQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();

            InitialMockSetup();

            _handler = new Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery.GetLearningDeliveriesEarningsQueryHandler(_dateTimeProvider.Object);
        }

        private void InitialMockSetup()
        {
            _dateTimeProvider
                .Setup(dtp => dtp.YearOfCollectionStart)
                .Returns(new DateTime(2017, 8, 1));

            _dateTimeProvider
                .Setup(dtp => dtp.Now)
                .Returns(new DateTime(2017, 12, 15, 12, 0, 0));
        }

        [Test]
        [TestCaseSource(nameof(LearningDeliveriesToProcessAndExpectedMonthlyAndCompletionAmounts))]
        public void ThenMonthlyInstallmentAndCompletionPaymentAreCalculatedCorrectly(IEnumerable<Data.Entities.LearningDeliveryToProcess> learningDeliveries, decimal monthly, decimal completion)
        {
            // Arrange
            _request = new GetLearningDeliveriesEarningsQueryRequest
            {
                LearningDeliveries = learningDeliveries
            };

            // Act
            var result = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(result.IsValid);
            Assert.IsTrue(result.IsValid);

            var learningDelivery = result.ProcessedLearningDeliveries.FirstOrDefault();

            Assert.IsNotNull(learningDelivery);
            Assert.AreEqual(monthly, learningDelivery.MonthlyInstallmentUncapped);
            Assert.AreEqual(completion, learningDelivery.CompletionPaymentUncapped);
        }

        [Test]
        [TestCaseSource(nameof(LearningDeliveriesToProcessWithSubmissionDatesAndExpectedPaymentSchedules))]
        public void ThenPeriodisedValuesAreCalculatedCorrectlyForTheProvidedSubmissionDate(IEnumerable<Data.Entities.LearningDeliveryToProcess> learningDeliveries, DateTime submissionDate, decimal[] expectedPeriodisedValues)
        {
            // Arrange
            _dateTimeProvider
                .Setup(dtp => dtp.Today)
                .Returns(submissionDate);

            _request = new GetLearningDeliveriesEarningsQueryRequest
            {
                LearningDeliveries = learningDeliveries
            };

            // Act
            var result = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(result.IsValid);
            Assert.IsTrue(result.IsValid);

            var periodisedValues = result.ProcessedLearningDeliveryPeriodisedValues.FirstOrDefault();

            Assert.IsNotNull(periodisedValues);

            for (var x = 0; x < 12; x++)
            {
                Assert.AreEqual(expectedPeriodisedValues[x], periodisedValues.GetPeriodValue(x + 1));
            }
        }
    }
}
using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery;
using SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDeliveryPeriodisedValues;
using SFA.DAS.CollectionEarnings.Calculator.Tools.Providers;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery.GetLearningDeliveriesEarningsQueryHandler
{
    public class WhenHandlingInLearningOrOnTimeFinishers
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
                923.07692m,
                3000.00m
            }
        };

        private static readonly object[] LearningDeliveriesToProcessWithSubmissionDatesAndExpectedOnProgrammeSchedule =
        {
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().Build()},
                new DateTime(2017, 9, 30),
                new[] {0.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m},
                "expecting monthly installments of 1000.00 starting from the second period for a learning episode that starts on 01/09/2017, runs for 12 months, has a negociated price of 15000 and is submitted on 30/09/2017."
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().Build()},
                new DateTime(2018, 7, 15),
                new[] {0.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m},
                "expecting monthly installments of 1000.00 starting from the second period for a learning episode that starts on 01/09/2017, runs for 12 months, has a negociated price of 15000 and is submitted on 15/07/2018."
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 30)).Build()},
                new DateTime(2018, 7, 15),
                new[] { 0.00m, 923.07692m, 923.07692m, 923.07692m, 923.07692m, 923.07692m, 923.07692m, 923.07692m, 923.07692m, 923.07692m, 923.07692m, 923.07692m},
                "expecting monthly installments of 923.07692 starting from the second period for a learning episode that starts on 01/09/2017, runs for 13 months, has a negociated price of 15000 and is submitted on 15/07/2018."
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().WithLearnStartDate(new DateTime(2017, 10, 1)).WithLearnPlanEndDate(new DateTime(2018, 3, 31)).WithLearnActEndDate(new DateTime(2018, 3, 31)).WithNegotiatedPrice(3750).Build()},
                new DateTime(2018, 3, 15),
                new[] {0.00m, 0.00m, 500.00m, 500.00m, 500.00m, 500.00m, 500.00m, 500.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting monthly installments of 500.00 starting from the third period for a learning episode that starts on 01/10/2017, runs for 6 months, ends on a census date, has a negociated price of 3750 and is submitted on 15/03/2018."
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().WithLearnStartDate(new DateTime(2016, 9, 1)).WithLearnPlanEndDate(new DateTime(2017, 9, 8)).WithLearnActEndDate(new DateTime(2017, 9, 8)).Build()},
                new DateTime(2017, 9, 30),
                new[] {1000.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting one monthly installment of 1000.00 in the first period for a learning episode that starts on 01/09/2016, runs for 12 months, does not end on a census date, has a negociated price of 15000 and is submitted on 30/09/2017."
            }
        };

        private static readonly object[] LearningDeliveriesToProcessWithSubmissionDatesAndExpectedCompletionSchedule =
        {
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().Build()},
                new DateTime(2017, 9, 30),
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting no completion payment for a learning episode that starts on 01/09/2017, runs for 12 months, has a negociated price of 15000 and is submitted on 30/09/2017."
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().WithLearnPlanEndDate(new DateTime(2018, 2, 28)).WithLearnActEndDate(new DateTime(2018, 2, 28)).Build()},
                new DateTime(2018, 3, 15),
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 3000.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting a completion payment of 3000.00 in in the 7th period for a learning episode that starts on 01/09/2017, runs for 6 months, ends on a census date, has a negociated price of 15000 and is submitted on 15/03/2018."
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().WithLearnStartDate(new DateTime(2017, 10, 1)).WithLearnPlanEndDate(new DateTime(2018, 3, 10)).WithLearnActEndDate(new DateTime(2018, 3, 10)).WithNegotiatedPrice(3750).Build()},
                new DateTime(2018, 3, 15),
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 750.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting a completion payment of 750.00m in the 8th period for a learning episode that starts on 01/10/2017, runs for 6 months, does not end on a census date, has a negociated price of 3750 and is submitted on 15/03/2018."
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().WithLearnStartDate(new DateTime(2016, 9, 1)).WithLearnPlanEndDate(new DateTime(2017, 9, 8)).WithLearnActEndDate(new DateTime(2017, 9, 8)).Build()},
                new DateTime(2017, 9, 30),
                new[] {0.00m, 3000.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting a completion payment of 3000.00 in the second period for a learning episode that starts on 01/09/2016, runs for 12 months, does not end on a census date, has a negociated price of 15000 and is submitted on 30/09/2017."
            }
        };

        private static readonly object[] LearningDeliveriesToProcessWithSubmissionDatesAndExpectedBalancingSchedule =
        {
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().Build()},
                new DateTime(2017, 9, 30),
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting no balancing payments for a learning episode that starts on 01/09/2017, runs for 12 months, has a negociated price of 15000 and is submitted on 30/09/2017."
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().Build()},
                new DateTime(2018, 7, 15),
                new[] { 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting no balancing payments for a learning episode that starts on 01/09/2017, runs for 12 months, has a negociated price of 15000 and is submitted on 15/07/2018."
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 30)).Build()},
                new DateTime(2018, 7, 15),
                new[] { 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting no balancing payments for a learning episode that starts on 01/09/2017, runs for 13 months, has a negociated price of 15000 and is submitted on 15/07/2018."
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().WithLearnStartDate(new DateTime(2017, 10, 1)).WithLearnPlanEndDate(new DateTime(2018, 3, 31)).WithLearnActEndDate(new DateTime(2018, 3, 31)).WithNegotiatedPrice(3750).Build()},
                new DateTime(2018, 3, 15),
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting no balancing payments for a learning episode that starts on 01/10/2017, runs for 6 months, ends on a census date, has a negociated price of 3750 and is submitted on 15/03/2018."
            },
            new object[]
            {
                new[] {new LearningDeliveryToProcessBuilder().WithLearnStartDate(new DateTime(2016, 9, 1)).WithLearnPlanEndDate(new DateTime(2017, 9, 8)).WithLearnActEndDate(new DateTime(2017, 9, 8)).Build()},
                new DateTime(2017, 9, 30),
                new[] { 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting no balancing payments for a learning episode that starts on 01/09/2016, runs for 12 months, does not end on a census date, has a negociated price of 15000 and is submitted on 30/09/2017."
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
        public void ThenMonthlyInstallmentAndCompletionPaymentAreCalculatedCorrectly(Infrastructure.Data.Entities.LearningDeliveryToProcess[] learningDeliveries, decimal monthly, decimal completion)
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
        [TestCaseSource(nameof(LearningDeliveriesToProcessWithSubmissionDatesAndExpectedOnProgrammeSchedule))]
        public void ThenPeriodisedOnProgrammeValuesAreCalculatedCorrectly(Infrastructure.Data.Entities.LearningDeliveryToProcess[] learningDeliveries, DateTime submissionDate, decimal[] expectedPeriodisedValues, string because)
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

            var periodisedValues = result.ProcessedLearningDeliveryPeriodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.OnProgrammePayment);

            Assert.IsNotNull(periodisedValues);

            for (var x = 0; x < 12; x++)
            {
                Assert.AreEqual(expectedPeriodisedValues[x], periodisedValues.GetPeriodValue(x + 1), because, null);
            }
        }

        [Test]
        [TestCaseSource(nameof(LearningDeliveriesToProcessWithSubmissionDatesAndExpectedCompletionSchedule))]
        public void ThenPeriodisedCompletionValuesAreCalculatedCorrectly(Infrastructure.Data.Entities.LearningDeliveryToProcess[] learningDeliveries, DateTime submissionDate, decimal[] expectedPeriodisedValues, string because)
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

            var periodisedValues = result.ProcessedLearningDeliveryPeriodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.CompletionPayment);

            Assert.IsNotNull(periodisedValues);

            for (var x = 0; x < 12; x++)
            {
                Assert.AreEqual(expectedPeriodisedValues[x], periodisedValues.GetPeriodValue(x + 1), because, null);
            }
        }

        [Test]
        [TestCaseSource(nameof(LearningDeliveriesToProcessWithSubmissionDatesAndExpectedBalancingSchedule))]
        public void ThenPeriodisedBalancingValuesAreCalculatedCorrectly(Infrastructure.Data.Entities.LearningDeliveryToProcess[] learningDeliveries, DateTime submissionDate, decimal[] expectedPeriodisedValues, string because)
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

            var periodisedValues = result.ProcessedLearningDeliveryPeriodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.BalancingPayment);

            Assert.IsNotNull(periodisedValues);

            for (var x = 0; x < 12; x++)
            {
                Assert.AreEqual(expectedPeriodisedValues[x], periodisedValues.GetPeriodValue(x + 1), because, null);
            }
        }
    }
}
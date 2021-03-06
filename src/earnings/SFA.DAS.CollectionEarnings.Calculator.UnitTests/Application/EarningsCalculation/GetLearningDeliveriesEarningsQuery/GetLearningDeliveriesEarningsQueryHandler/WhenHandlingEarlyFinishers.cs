﻿using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues;
using SFA.DAS.CollectionEarnings.Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery;
using SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess;
using SFA.DAS.CollectionEarnings.Calculator.Tools.Providers;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Builders;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery.GetLearningDeliveriesEarningsQueryHandler
{
    public class WhenHandlingEarlyFinishers
    {
        #region Test Case Sources

        private static readonly object[] LearningDeliveriesToProcessWithSubmissionAndYearOfCollectionDatesAndExpectedOnProgrammeSchedule =
        {
            new object[]
            {
                new[] {new LearningDeliveryBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 8)).WithLearnActEndDate(new DateTime(2018, 8, 8)).Build()},
                new DateTime(2018, 10, 15),
                new DateTime(2018, 8, 1),
                new[] {1000.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting a monthly installment of 1000.00 in the first period for a learning episode that starts on 01/09/2017, runs for 12 months, finishes early on 08/08/2018, has a negociated price of 15000 and is submitted on 15/10/2018."
            },
            new object[]
            {
                new[] {new LearningDeliveryBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 8)).WithLearnActEndDate(new DateTime(2018, 9, 1)).Build()},
                new DateTime(2018, 10, 15),
                new DateTime(2018, 8, 1),
                new[] {1000.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting a monthly installment of 1000.00 in the first period for a learning episode that starts on 01/09/2017, runs for 12 months, finishes early on 01/09/2018, has a negociated price of 15000 and is submitted on 15/10/2018."
            },
            new object[]
            {
                new[] {new LearningDeliveryBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 8)).WithLearnActEndDate(new DateTime(2018, 8, 8)).Build()},
                new DateTime(2017, 10, 15),
                new DateTime(2017, 8, 1),
                new[] {0.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m},
                "expecting monthly installments of 1000.00 starting from the second period for a learning episode that starts on 01/09/2017, runs for 12 months, finishes early on 08/08/2018, has a negociated price of 15000 and is submitted on 15/10/2017."
            }
        };

        private static readonly object[] LearningDeliveriesToProcessWithSubmissionAndYearOfCollectionDatesAndExpectedCompletionSchedule =
        {
            new object[]
            {
                new[] {new LearningDeliveryBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 8)).WithLearnActEndDate(new DateTime(2018, 8, 8)).Build()},
                new DateTime(2018, 10, 15),
                new DateTime(2018, 8, 1),
                new[] {3000.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting a completion payment of 3000.00 in the first period for a learning episode that starts on 01/09/2017, runs for 12 months, finishes early on 08/08/2018, has a negociated price of 15000 and is submitted on 15/10/2018."
            },
            new object[]
            {
                new[] {new LearningDeliveryBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 8)).WithLearnActEndDate(new DateTime(2018, 9, 1)).Build()},
                new DateTime(2018, 10, 15),
                new DateTime(2018, 8, 1),
                new[] {0.00m, 3000.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting a completion payment of 3000.00 in the second period for a learning episode that starts on 01/09/2017, runs for 12 months, finishes early on 01/09/2018, has a negociated price of 15000 and is submitted on 15/10/2018."
            },
            new object[]
            {
                new[] {new LearningDeliveryBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 8)).WithLearnActEndDate(new DateTime(2018, 8, 8)).Build()},
                new DateTime(2017, 10, 15),
                new DateTime(2017, 8, 1),
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting no completion payments for a learning episode that starts on 01/09/2017, runs for 12 months, finishes early on 08/08/2018, has a negociated price of 15000 and is submitted on 15/10/2017."
            }
        };

        private static readonly object[] LearningDeliveriesToProcessWithSubmissionAndYearOfCollectionDatesAndExpectedBalancingSchedule =
        {
            new object[]
            {
                new[] {new LearningDeliveryBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 8)).WithLearnActEndDate(new DateTime(2018, 8, 8)).Build()},
                new DateTime(2018, 10, 15),
                new DateTime(2018, 8, 1),
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting no balancing payment for a learning episode that starts on 01/09/2017, runs for 12 months, finishes early on 08/08/2018, has a negociated price of 15000 and is submitted on 15/10/2018."
            },
            new object[]
            {
                new[] {new LearningDeliveryBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 8)).WithLearnActEndDate(new DateTime(2018, 7, 8)).Build()},
                new DateTime(2017, 10, 15),
                new DateTime(2017, 8, 1),
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 1000.00m},
                "expecting a balancing payment of 1000.00 in the 12th period for a learning episode that starts on 01/09/2017, runs for 12 months, finishes early on 08/07/2018, has a negociated price of 15000 and is submitted on 15/10/2017."
            },
            new object[]
            {
                new[] {new LearningDeliveryBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 8)).WithLearnActEndDate(new DateTime(2018, 6, 8)).Build()},
                new DateTime(2017, 10, 15),
                new DateTime(2017, 8, 1),
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 2000.00m, 0.00m },
                "expecting a balancing payment of 2000.00 in the 12th period for a learning episode that starts on 01/09/2017, runs for 12 months, finishes early on 08/06/2018, has a negociated price of 15000 and is submitted on 15/10/2017."
            },
            new object[]
            {
                new[] {new LearningDeliveryBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 8)).WithLearnActEndDate(new DateTime(2018, 9, 1)).Build()},
                new DateTime(2018, 10, 15),
                new DateTime(2018, 8, 1),
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting no balancing payment for a learning episode that starts on 01/09/2017, runs for 12 months, finishes early on 01/09/2018, has a negociated price of 15000 and is submitted on 15/10/2018."
            },
            new object[]
            {
                new[] {new LearningDeliveryBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 8)).WithLearnActEndDate(new DateTime(2018, 8, 8)).Build()},
                new DateTime(2017, 10, 15),
                new DateTime(2017, 8, 1),
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                "expecting no balancing payment for a learning episode that starts on 01/09/2017, runs for 12 months, finishes early on 08/08/2018, has a negociated price of 15000 and is submitted on 15/10/2017."
            }
        };

        private static readonly object[] LearningDeliveriesToProcessWithSubmissionAndYearOfCollectionDatesAndExpectedPeriodsSchedule =
        {
            new object[]
            {
                new[] {new LearningDeliveryBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 8)).WithLearnActEndDate(new DateTime(2018, 8, 8)).Build()},
                new DateTime(2018, 10, 15),
                new DateTime(2018, 8, 1),
                new[] {1000.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                new[] {3000.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m}
            },
            new object[]
            {
                new[] {new LearningDeliveryBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 8)).WithLearnActEndDate(new DateTime(2018, 7, 8)).Build()},
                new DateTime(2017, 10, 15),
                new DateTime(2017, 8, 1),
                new[] {0.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m},
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 3000.00m},
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 1000.00m}
            },
            new object[]
            {
                new[] {new LearningDeliveryBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 8)).WithLearnActEndDate(new DateTime(2018, 6, 8)).Build()},
                new DateTime(2017, 10, 15),
                new DateTime(2017, 8, 1),
                new[] {0.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 0.00m},
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 3000.00m, 0.00m },
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 2000.00m, 0.00m }
            },
            new object[]
            {
                new[] {new LearningDeliveryBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 8)).WithLearnActEndDate(new DateTime(2018, 9, 1)).Build()},
                new DateTime(2018, 10, 15),
                new DateTime(2018, 8, 1),
                new[] {1000.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                new[] {0.00m, 3000.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m}
            },
            new object[]
            {
                new[] {new LearningDeliveryBuilder().WithLearnPlanEndDate(new DateTime(2018, 9, 8)).WithLearnActEndDate(new DateTime(2018, 8, 8)).Build()},
                new DateTime(2017, 10, 15),
                new DateTime(2017, 8, 1),
                new[] {0.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m, 1000.00m},
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m},
                new[] {0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m}
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
        [TestCaseSource(nameof(LearningDeliveriesToProcessWithSubmissionAndYearOfCollectionDatesAndExpectedOnProgrammeSchedule))]
        public void ThenPeriodisedOnProgrammeValuesAreCalculatedCorrectly(LearningDelivery[] learningDeliveries, DateTime submissionDate, DateTime yearOfCollectionDate, decimal[] expectedPeriodisedValues, string because)
        {
            // Arrange
            _dateTimeProvider
                .Setup(dtp => dtp.YearOfCollectionStart)
                .Returns(yearOfCollectionDate);

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

            var periodisedValues =
                result.PriceEpisodesPeriodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.PriceEpisodeOnProgPayment);

            Assert.IsNotNull(periodisedValues);

            for (var x = 0; x < 12; x++)
            {
                Assert.AreEqual(expectedPeriodisedValues[x], periodisedValues.GetPeriodValue(x + 1), because);
            }
        }

        [Test]
        [TestCaseSource(nameof(LearningDeliveriesToProcessWithSubmissionAndYearOfCollectionDatesAndExpectedCompletionSchedule))]
        public void ThenPeriodisedCompletionValuesAreCalculatedCorrectly(LearningDelivery[] learningDeliveries, DateTime submissionDate, DateTime yearOfCollectionDate, decimal[] expectedPeriodisedValues, string because)
        {
            // Arrange
            _dateTimeProvider
                .Setup(dtp => dtp.YearOfCollectionStart)
                .Returns(yearOfCollectionDate);

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

            var periodisedValues =
                result.PriceEpisodesPeriodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.PriceEpisodeCompletionPayment);

            Assert.IsNotNull(periodisedValues);

            for (var x = 0; x < 12; x++)
            {
                Assert.AreEqual(expectedPeriodisedValues[x], periodisedValues.GetPeriodValue(x + 1), because);
            }
        }

        [Test]
        [TestCaseSource(nameof(LearningDeliveriesToProcessWithSubmissionAndYearOfCollectionDatesAndExpectedBalancingSchedule))]
        public void ThenPeriodisedBalancingValuesAreCalculatedCorrectly(LearningDelivery[] learningDeliveries, DateTime submissionDate, DateTime yearOfCollectionDate, decimal[] expectedPeriodisedValues, string because)
        {
            // Arrange
            _dateTimeProvider
                .Setup(dtp => dtp.YearOfCollectionStart)
                .Returns(yearOfCollectionDate);

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

            var periodisedValues =
                result.PriceEpisodesPeriodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.PriceEpisodeBalancePayment);

            Assert.IsNotNull(periodisedValues);

            for (var x = 0; x < 12; x++)
            {
                Assert.AreEqual(expectedPeriodisedValues[x], periodisedValues.GetPeriodValue(x + 1), because);
            }
        }

        [Test]
        [TestCaseSource(nameof(LearningDeliveriesToProcessWithSubmissionAndYearOfCollectionDatesAndExpectedPeriodsSchedule))]
        public void ThenPeriodEarningsAreCalculatedCorrectly(LearningDelivery[] learningDeliveries, DateTime submissionDate, DateTime yearOfCollectionDate, decimal[] expectedOnProgramme, decimal[] expectedCompletion, decimal[] expectedBalancing)
        {
            // Arrange
            _dateTimeProvider
                .Setup(dtp => dtp.YearOfCollectionStart)
                .Returns(yearOfCollectionDate);

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
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);

            var periods = result.PriceEpisodesPeriodsEarnings;

            Assert.IsNotNull(periods);
            Assert.AreEqual(12, periods.Length);

            for (var x = 0; x < 12; x++)
            {
                var periodEarning = periods.SingleOrDefault(pe => pe.Period == x + 1);

                Assert.IsNotNull(periodEarning);

                Assert.AreEqual(expectedOnProgramme[x], periodEarning.PriceEpisodeOnProgPayment);
                Assert.AreEqual(expectedCompletion[x], periodEarning.PriceEpisodeCompletionPayment);
                Assert.AreEqual(expectedBalancing[x], periodEarning.PriceEpisodeBalancePayment);
            }
        }
    }
}
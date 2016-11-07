using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDeliveryPeriod;
using SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDeliveryPeriodisedValues;
using SFA.DAS.CollectionEarnings.Calculator.Tools.Extensions;
using SFA.DAS.CollectionEarnings.Calculator.Tools.Providers;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery
{
    public class GetLearningDeliveriesEarningsQueryHandler : IRequestHandler<GetLearningDeliveriesEarningsQueryRequest, GetLearningDeliveriesEarningsQueryResponse>
    {
        private const decimal CompletionPaymentRatio = 0.2m;
        private const decimal InLearningPaymentRatio = 1.00m - CompletionPaymentRatio;

        private readonly IDateTimeProvider _dateTimeProvider;

        private List<Infrastructure.Data.Entities.ProcessedLearningDelivery> _processedLearningDeliveries;
        private List<Infrastructure.Data.Entities.ProcessedLearningDeliveryPeriodisedValues> _processedLearningDeliveryPeriodisedValues;
        private List<LearningDeliveryPeriodEarning> _learningDeliveryPeriodEarnings;

        public GetLearningDeliveriesEarningsQueryHandler(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public GetLearningDeliveriesEarningsQueryResponse Handle(GetLearningDeliveriesEarningsQueryRequest message)
        {
            _processedLearningDeliveries = new List<Infrastructure.Data.Entities.ProcessedLearningDelivery>();
            _processedLearningDeliveryPeriodisedValues = new List<Infrastructure.Data.Entities.ProcessedLearningDeliveryPeriodisedValues>();
            _learningDeliveryPeriodEarnings = new List<LearningDeliveryPeriodEarning>();

            try
            {
                var learningDeliveries = message.LearningDeliveries.ToList();

                learningDeliveries.ForEach(
                    learningDelivery =>
                    {
                        var completionPaymentUncapped = CalculateCompletionPayment(learningDelivery);
                        var monthlyInstallmentUncapped = CalculateMonthlyInstallment(learningDelivery);

                        BuildProcessedLearningDelivery(
                            learningDelivery,
                            monthlyInstallmentUncapped,
                            monthlyInstallmentUncapped,
                            completionPaymentUncapped,
                            completionPaymentUncapped);

                        BuildPeriodEarningsAndPeriodisedValues(
                            learningDelivery,
                            monthlyInstallmentUncapped,
                            completionPaymentUncapped);
                    });

                return new GetLearningDeliveriesEarningsQueryResponse
                {
                    IsValid = true,
                    ProcessedLearningDeliveries = _processedLearningDeliveries.ToArray(),
                    ProcessedLearningDeliveryPeriodisedValues = _processedLearningDeliveryPeriodisedValues.ToArray(),
                    LearningDeliveryPeriodEarnings = _learningDeliveryPeriodEarnings.ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetLearningDeliveriesEarningsQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }

        private int CalculateNumberOfPeriods(Infrastructure.Data.Entities.LearningDeliveryToProcess learningDelivery)
        {
            var result = 0;

            var censusDate = learningDelivery.LearnStartDate.LastDayOfMonth();

            while (censusDate <= learningDelivery.LearnPlanEndDate)
            {
                censusDate = censusDate.AddMonths(1).LastDayOfMonth();
                result++;
            }

            return result;
        }

        private decimal CalculateCompletionPayment(Infrastructure.Data.Entities.LearningDeliveryToProcess learningDelivery)
        {
            return decimal.Round(learningDelivery.NegotiatedPrice * CompletionPaymentRatio, 5);
        }

        private decimal CalculateMonthlyInstallment(Infrastructure.Data.Entities.LearningDeliveryToProcess learningDelivery)
        {
            return decimal.Round(learningDelivery.NegotiatedPrice * InLearningPaymentRatio / CalculateNumberOfPeriods(learningDelivery), 5);
        }

        private DateTime CalculateFirstCensusDateForTheLearningDelivery(Infrastructure.Data.Entities.LearningDeliveryToProcess learningDelivery)
        {
            var firstCensusDateAfterYearOfCollectionStart = _dateTimeProvider.YearOfCollectionStart.LastDayOfMonth();
            var firstCensusDateAfterLearningStart = learningDelivery.LearnStartDate.LastDayOfMonth();

            return firstCensusDateAfterYearOfCollectionStart < firstCensusDateAfterLearningStart
                ? firstCensusDateAfterLearningStart
                : firstCensusDateAfterYearOfCollectionStart;
        }

        private int CalculateFirstPeriodForTheLearningDelivery(Infrastructure.Data.Entities.LearningDeliveryToProcess learningDelivery)
        {
            var firstDayAfterYearOfCollectionStart = _dateTimeProvider.YearOfCollectionStart.FirstDayOfMonth();

            var period = learningDelivery.LearnStartDate.Month - firstDayAfterYearOfCollectionStart.Month
                         + 12 * (learningDelivery.LearnStartDate.Year - firstDayAfterYearOfCollectionStart.Year)
                         + 1;

            return period < 0
                ? 1
                : period;
        }

        private void BuildProcessedLearningDelivery(Infrastructure.Data.Entities.LearningDeliveryToProcess learningDelivery, decimal monthlyInstallment, decimal monthlyInstallmentUncapped, decimal completionPayment, decimal completionPaymentUncapped)
        {
            _processedLearningDeliveries.Add(
                new Infrastructure.Data.Entities.ProcessedLearningDelivery
                {
                    Ukprn = learningDelivery.Ukprn,
                    LearnRefNumber = learningDelivery.LearnRefNumber,
                    Uln = learningDelivery.Uln,
                    NiNumber = learningDelivery.NiNumber,
                    AimSeqNumber = learningDelivery.AimSeqNumber,
                    StdCode = learningDelivery.StandardCode,
                    ProgType = learningDelivery.ProgrammeType,
                    FworkCode = learningDelivery.FrameworkCode,
                    PwayCode = learningDelivery.PathwayCode,
                    NegotiatedPrice = learningDelivery.NegotiatedPrice,
                    LearnStartDate = learningDelivery.LearnStartDate,
                    OrigLearnStartDate = learningDelivery.OrigLearnStartDate,
                    LearnPlanEndDate = learningDelivery.LearnPlanEndDate,
                    LearnActEndDate = learningDelivery.LearnActEndDate,
                    MonthlyInstallment = monthlyInstallment,
                    MonthlyInstallmentUncapped = monthlyInstallmentUncapped,
                    CompletionPayment = completionPayment,
                    CompletionPaymentUncapped = completionPaymentUncapped
                });
        }

        private void BuildPeriodEarningsAndPeriodisedValues(Infrastructure.Data.Entities.LearningDeliveryToProcess learningDelivery, decimal monthlyInstallment, decimal completionPayment)
        {
            var periodEarnings = new List<LearningDeliveryPeriodEarning>();

            var onProgrammeEarning = new Infrastructure.Data.Entities.ProcessedLearningDeliveryPeriodisedValues
            {
                LearnRefNumber = learningDelivery.LearnRefNumber,
                AimSeqNumber = learningDelivery.AimSeqNumber,
                AttributeName = AttributeNames.OnProgrammePayment
            };

            var completionEarning = new Infrastructure.Data.Entities.ProcessedLearningDeliveryPeriodisedValues
            {
                LearnRefNumber = learningDelivery.LearnRefNumber,
                AimSeqNumber = learningDelivery.AimSeqNumber,
                AttributeName = AttributeNames.CompletionPayment
            };

            var balancingEarning = new Infrastructure.Data.Entities.ProcessedLearningDeliveryPeriodisedValues
            {
                LearnRefNumber = learningDelivery.LearnRefNumber,
                AimSeqNumber = learningDelivery.AimSeqNumber,
                AttributeName = AttributeNames.BalancingPayment
            };

            var shouldAddCompletionPayment = learningDelivery.LearnActEndDate.HasValue;
            var shouldAddBalancingPayment = shouldAddCompletionPayment &&
                                            learningDelivery.LearnActEndDate.Value < learningDelivery.LearnPlanEndDate;

            var censusDate = CalculateFirstCensusDateForTheLearningDelivery(learningDelivery);
            var period = CalculateFirstPeriodForTheLearningDelivery(learningDelivery);

            var plannedEndDate = learningDelivery.LearnPlanEndDate;
            var learningEndCensusDate = learningDelivery.LearnActEndDate?.LastDayOfMonth() ?? plannedEndDate.LastDayOfMonth();

            while (censusDate <= learningEndCensusDate && period <= 12)
            {
                var periodEarning = new LearningDeliveryPeriodEarning
                {
                    LearnerReferenceNumber = learningDelivery.LearnRefNumber,
                    AimSequenceNumber = learningDelivery.AimSeqNumber,
                    Period = period
                };

                if (censusDate <= plannedEndDate)
                {
                    onProgrammeEarning.SetPeriodValue(period, monthlyInstallment);

                    periodEarning.OnProgrammeEarning = monthlyInstallment;
                }

                if (shouldAddCompletionPayment && censusDate == learningEndCensusDate)
                {
                    completionEarning.SetPeriodValue(period, completionPayment);

                    periodEarning.CompletionEarning = completionPayment;
                }

                if (shouldAddBalancingPayment && censusDate == learningEndCensusDate)
                {
                    var balancingPayment = CalculateBalancingPaymentAmount(monthlyInstallment, completionPayment, learningDelivery);

                    balancingEarning.SetPeriodValue(period, balancingPayment);

                    periodEarning.BalancingEarning = balancingPayment;
                }

                periodEarnings.Add(periodEarning);

                censusDate = censusDate.AddMonths(1).LastDayOfMonth();
                period++;
            }

            FillMissingPeriodEarnings(periodEarnings, learningDelivery);

            _processedLearningDeliveryPeriodisedValues.Add(onProgrammeEarning);
            _processedLearningDeliveryPeriodisedValues.Add(completionEarning);
            _processedLearningDeliveryPeriodisedValues.Add(balancingEarning);

            _learningDeliveryPeriodEarnings.AddRange(periodEarnings);
        }

        private decimal CalculateBalancingPaymentAmount(decimal monthlyInstallment, decimal completionPayment, Infrastructure.Data.Entities.LearningDeliveryToProcess learningDelivery)
        {
            if (!learningDelivery.LearnActEndDate.HasValue)
            {
                return 0.00m;
            }

            var numberOfPeriods = CalculateNumberOfPeriods(learningDelivery);
            var numberOfPeriodsToBalance = CalculateNumberOfPeriodsToBalance(learningDelivery.LearnPlanEndDate, learningDelivery.LearnActEndDate.Value);

            var amountEarnedSoFar = monthlyInstallment * (numberOfPeriods - numberOfPeriodsToBalance);

            return decimal.Round(learningDelivery.NegotiatedPrice - completionPayment - amountEarnedSoFar, 5);
        }

        private int CalculateNumberOfPeriodsToBalance(DateTime plannedEndDate, DateTime actualEndDate)
        {
            var result = 0;

            var censusDate = actualEndDate.AddMonths(1).LastDayOfMonth();

            while (censusDate <= plannedEndDate)
            {
                censusDate = censusDate.AddMonths(1).LastDayOfMonth();
                result++;
            }

            return result;
        }


        private void FillMissingPeriodEarnings(List<LearningDeliveryPeriodEarning>  periodEarnings, Infrastructure.Data.Entities.LearningDeliveryToProcess learningDelivery)
        {
            if (periodEarnings.Count == 12)
            {
                return;
            }

            for (var period = 1; period <= 12; period++)
            {
                if (periodEarnings.Count(pe => pe.Period == period) == 1)
                {
                    continue;
                }

                _learningDeliveryPeriodEarnings.Add(new LearningDeliveryPeriodEarning
                {
                    LearnerReferenceNumber = learningDelivery.LearnRefNumber,
                    AimSequenceNumber = learningDelivery.AimSeqNumber,
                    Period = period
                });
            }
        }
    }
}
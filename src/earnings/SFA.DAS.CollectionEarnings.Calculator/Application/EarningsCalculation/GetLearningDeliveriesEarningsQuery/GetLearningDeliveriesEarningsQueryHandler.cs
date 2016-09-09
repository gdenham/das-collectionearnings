using System;
using System.Collections.Concurrent;
using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.Calculator.Tools.Extensions;
using SFA.DAS.CollectionEarnings.Calculator.Tools.Providers;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery
{
    public class GetLearningDeliveriesEarningsQueryHandler : IRequestHandler<GetLearningDeliveriesEarningsQueryRequest, GetLearningDeliveriesEarningsQueryResponse>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetLearningDeliveriesEarningsQueryHandler(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public GetLearningDeliveriesEarningsQueryResponse Handle(GetLearningDeliveriesEarningsQueryRequest message)
        {
            try
            {
                var processedLearningDeliveries = new ConcurrentBag<Data.Entities.ProcessedLearningDelivery>();
                var processedLearningDeliveryPeriodisedValues = new ConcurrentBag<Data.Entities.ProcessedLearningDeliveryPeriodisedValues>();

                var learningDeliveries = message.LearningDeliveries.ToList();

                learningDeliveries.ForEach(learningDelivery =>
                {
                    var completionPaymentUncapped = CalculateCompletionPayment(learningDelivery);
                    var monthlyInstallmentUncapped = CalculateMonthlyInstallment(learningDelivery);

                    processedLearningDeliveries.Add(
                        BuildProcessedLearningDelivery(
                            learningDelivery,
                            monthlyInstallmentUncapped,
                            monthlyInstallmentUncapped,
                            completionPaymentUncapped,
                            completionPaymentUncapped));

                    processedLearningDeliveryPeriodisedValues.Add(
                        BuildProcessedLearningDeliveryPeriodisedValues(
                            learningDelivery,
                            monthlyInstallmentUncapped,
                            completionPaymentUncapped));
                });

                return new GetLearningDeliveriesEarningsQueryResponse
                {
                    IsValid = true,
                    ProcessedLearningDeliveries = processedLearningDeliveries,
                    ProcessedLearningDeliveryPeriodisedValues = processedLearningDeliveryPeriodisedValues
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

        private int CalculateNumberOfPeriods(Data.Entities.LearningDeliveryToProcess learningDelivery)
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

        private decimal CalculateCompletionPayment(Data.Entities.LearningDeliveryToProcess learningDelivery)
        {
            return learningDelivery.NegotiatedPrice * 0.2m;
        }

        private decimal CalculateMonthlyInstallment(Data.Entities.LearningDeliveryToProcess learningDelivery)
        {
            return learningDelivery.NegotiatedPrice * 0.8m / CalculateNumberOfPeriods(learningDelivery);
        }

        private DateTime CalculateFirstCensusDateForTheLearningDelivery(Data.Entities.LearningDeliveryToProcess learningDelivery)
        {
            var firstCensusDateAfterYearOfCollectionStart = _dateTimeProvider.YearOfCollectionStart.LastDayOfMonth();
            var firstCensusDateAfterLearningStart = learningDelivery.LearnStartDate.LastDayOfMonth();

            return firstCensusDateAfterYearOfCollectionStart < firstCensusDateAfterLearningStart
                ? firstCensusDateAfterLearningStart
                : firstCensusDateAfterYearOfCollectionStart;
        }

        private int CalculateFirstPeriodForTheLearningDelivery(Data.Entities.LearningDeliveryToProcess learningDelivery)
        {
            var firstDayAfterYearOfCollectionStart = _dateTimeProvider.YearOfCollectionStart.FirstDayOfMonth();

            var period = learningDelivery.LearnStartDate.Month - firstDayAfterYearOfCollectionStart.Month
                         + 12 * (learningDelivery.LearnStartDate.Year - firstDayAfterYearOfCollectionStart.Year)
                         + 1;

            return period < 0
                ? 1
                : period;
        }

        private Data.Entities.ProcessedLearningDelivery BuildProcessedLearningDelivery(Data.Entities.LearningDeliveryToProcess learningDelivery, decimal monthlyInstallment, decimal monthlyInstallmentUncapped, decimal completionPayment, decimal completionPaymentUncapped)
        {
            return new Data.Entities.ProcessedLearningDelivery
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
            };
        }

        private Data.Entities.ProcessedLearningDeliveryPeriodisedValues BuildProcessedLearningDeliveryPeriodisedValues(Data.Entities.LearningDeliveryToProcess learningDelivery, decimal monthlyInstallment, decimal completionPayment)
        {
            var result = new Data.Entities.ProcessedLearningDeliveryPeriodisedValues
            {
                LearnRefNumber = learningDelivery.LearnRefNumber,
                AimSeqNumber = learningDelivery.AimSeqNumber,
                Period_1 = 0m,
                Period_2 = 0m,
                Period_3 = 0m,
                Period_4 = 0m,
                Period_5 = 0m,
                Period_6 = 0m,
                Period_7 = 0m,
                Period_8 = 0m,
                Period_9 = 0m,
                Period_10 = 0m,
                Period_11 = 0m,
                Period_12 = 0m
            };

            var addedCompletionPayment = false;
            var shouldAddCompletionPayment = learningDelivery.LearnActEndDate.HasValue;

            var censusDate = CalculateFirstCensusDateForTheLearningDelivery(learningDelivery);
            var period = CalculateFirstPeriodForTheLearningDelivery(learningDelivery);

            var lastDate = learningDelivery.LearnActEndDate.HasValue
                ? learningDelivery.LearnActEndDate
                : learningDelivery.LearnPlanEndDate;

            while (censusDate <= lastDate && period <= 12)
            {
                var amount = monthlyInstallment;

                if (censusDate == lastDate && shouldAddCompletionPayment)
                {
                    amount += completionPayment;
                    addedCompletionPayment = true;
                }

                result.SetPeriodValue(period, amount);

                censusDate = censusDate.AddMonths(1).LastDayOfMonth();
                period++;
            }

            if (shouldAddCompletionPayment && !addedCompletionPayment && period < 12)
            {
                result.SetPeriodValue(period, completionPayment);
            }

            return result;
        }
    }
}
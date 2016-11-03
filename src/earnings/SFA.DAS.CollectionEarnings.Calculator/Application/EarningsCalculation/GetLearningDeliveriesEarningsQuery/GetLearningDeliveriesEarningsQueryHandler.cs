using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
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

        public GetLearningDeliveriesEarningsQueryHandler(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public GetLearningDeliveriesEarningsQueryResponse Handle(GetLearningDeliveriesEarningsQueryRequest message)
        {
            try
            {
                var processedLearningDeliveries = new List<Infrastructure.Data.Entities.ProcessedLearningDelivery>();
                var processedLearningDeliveryPeriodisedValues = new List<Infrastructure.Data.Entities.ProcessedLearningDeliveryPeriodisedValues>();

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

                    processedLearningDeliveryPeriodisedValues.AddRange(
                        BuildProcessedLearningDeliveryPeriodisedValues(
                            learningDelivery,
                            monthlyInstallmentUncapped,
                            completionPaymentUncapped));
                });

                return new GetLearningDeliveriesEarningsQueryResponse
                {
                    IsValid = true,
                    ProcessedLearningDeliveries = processedLearningDeliveries.ToArray(),
                    ProcessedLearningDeliveryPeriodisedValues = processedLearningDeliveryPeriodisedValues.ToArray()
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

        private Infrastructure.Data.Entities.ProcessedLearningDelivery BuildProcessedLearningDelivery(Infrastructure.Data.Entities.LearningDeliveryToProcess learningDelivery, decimal monthlyInstallment, decimal monthlyInstallmentUncapped, decimal completionPayment, decimal completionPaymentUncapped)
        {
            return new Infrastructure.Data.Entities.ProcessedLearningDelivery
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

        private Infrastructure.Data.Entities.ProcessedLearningDeliveryPeriodisedValues[] BuildProcessedLearningDeliveryPeriodisedValues(Infrastructure.Data.Entities.LearningDeliveryToProcess learningDelivery, decimal monthlyInstallment, decimal completionPayment)
        {
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

            var censusDate = CalculateFirstCensusDateForTheLearningDelivery(learningDelivery);
            var period = CalculateFirstPeriodForTheLearningDelivery(learningDelivery);

            var plannedEndDate = learningDelivery.LearnPlanEndDate;
            var learningEndCensusDate = learningDelivery.LearnActEndDate?.LastDayOfMonth() ?? plannedEndDate.LastDayOfMonth();

            while (censusDate <= learningEndCensusDate && period <= 12)
            {
                if (censusDate <= plannedEndDate)
                {
                    onProgrammeEarning.SetPeriodValue(period, monthlyInstallment);
                }

                if (shouldAddCompletionPayment && censusDate == learningEndCensusDate)
                {
                    completionEarning.SetPeriodValue(period, completionPayment);
                }

                censusDate = censusDate.AddMonths(1).LastDayOfMonth();
                period++;
            }

            return new [] { onProgrammeEarning, completionEarning, balancingEarning };
        }
    }
}
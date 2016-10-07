using System;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Entities
{
    public class ProcessedLearningDeliveryBuilder : IEntityBuilder<ProcessedLearningDelivery>
    {
        private int _ukprn = 10007459;
        private string _learnRefNumber = "Lrn001";
        private long _uln = 1000000019;
        private string _niNumber = "AB123456C";
        private int _aimSeqNumber = 1;
        private long? _stdCode;
        private int? _progType = 20;
        private int? _fworkCode = 550;
        private int? _pwayCode = 6;
        private DateTime _learnStartDate = new DateTime(2016, 9, 1);
        private DateTime? _origLearnStartDate;
        private DateTime _learnPlanEndDate = new DateTime(2018, 12, 31);
        private DateTime? _learnActEndDate;
        private int _negotiatedPrice = 15000;
        private decimal _monthlyInstallment = 1000m;
        private decimal _monthlyInstallmentUncapped = 1000m;
        private decimal _completionPayment = 3000m;
        private decimal _completionPaymentUncapped = 3000m;

        public ProcessedLearningDelivery Build()
        {
            return new ProcessedLearningDelivery
            {
                Ukprn = _ukprn,
                LearnRefNumber = _learnRefNumber,
                Uln = _uln,
                NiNumber = _niNumber,
                AimSeqNumber = _aimSeqNumber,
                StdCode = _stdCode,
                ProgType = _progType,
                FworkCode = _fworkCode,
                PwayCode = _pwayCode,
                LearnStartDate = _learnStartDate,
                OrigLearnStartDate = _origLearnStartDate,
                LearnPlanEndDate = _learnPlanEndDate,
                LearnActEndDate = _learnActEndDate,
                NegotiatedPrice = _negotiatedPrice,
                MonthlyInstallment = _monthlyInstallment,
                MonthlyInstallmentUncapped = _monthlyInstallmentUncapped,
                CompletionPayment = _completionPayment,
                CompletionPaymentUncapped = _completionPaymentUncapped
            };
        }

        public ProcessedLearningDeliveryBuilder WithUkprn(int ukprn)
        {
            _ukprn = ukprn;

            return this;
        }

        public ProcessedLearningDeliveryBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public ProcessedLearningDeliveryBuilder WithUln(long uln)
        {
            _uln = uln;

            return this;
        }

        public ProcessedLearningDeliveryBuilder WithNiNumber(string niNumber)
        {
            _niNumber = niNumber;

            return this;
        }

        public ProcessedLearningDeliveryBuilder WithAimSeqNumber(int aimseqNumber)
        {
            _aimSeqNumber = aimseqNumber;

            return this;
        }

        public ProcessedLearningDeliveryBuilder WithStandardCode(long? standardCode)
        {
            _stdCode = standardCode;

            return this;
        }

        public ProcessedLearningDeliveryBuilder WithProgrammeType(int? programmeType)
        {
            _progType = programmeType;

            return this;
        }

        public ProcessedLearningDeliveryBuilder WithFrameworkCode(int? frameworkCode)
        {
            _fworkCode = frameworkCode;

            return this;
        }

        public ProcessedLearningDeliveryBuilder WithPathwayCode(int? pathwayCode)
        {
            _pwayCode = pathwayCode;

            return this;
        }

        public ProcessedLearningDeliveryBuilder WithLearnStartDate(DateTime learnStartDate)
        {
            _learnStartDate = learnStartDate;

            return this;
        }

        public ProcessedLearningDeliveryBuilder WithOrigLearnStartDate(DateTime? origLearnStartDate)
        {
            _origLearnStartDate = origLearnStartDate;

            return this;
        }

        public ProcessedLearningDeliveryBuilder WithLearnPlanEndDate(DateTime learnPlanEndDate)
        {
            _learnPlanEndDate = learnPlanEndDate;

            return this;
        }

        public ProcessedLearningDeliveryBuilder WithLearnActEndDate(DateTime? learnActEndDate)
        {
            _learnActEndDate = learnActEndDate;

            return this;
        }

        public ProcessedLearningDeliveryBuilder WithNegotiatedPrice(int negotiatedPrice)
        {
            _negotiatedPrice = negotiatedPrice;

            return this;
        }

        public ProcessedLearningDeliveryBuilder WithMonthlyInstallment(decimal monthlyInstallment)
        {
            _monthlyInstallment = monthlyInstallment;

            return this;
        }

        public ProcessedLearningDeliveryBuilder WithMonthlyInstallmentUncapped(decimal monthlyInstallmentUncapped)
        {
            _monthlyInstallmentUncapped = monthlyInstallmentUncapped;

            return this;
        }

        public ProcessedLearningDeliveryBuilder WithCompletionPayment(decimal completionPayment)
        {
            _completionPayment = completionPayment;

            return this;
        }

        public ProcessedLearningDeliveryBuilder WithCompletionPaymentUncapped(decimal completionPaymentUncapped)
        {
            _completionPaymentUncapped = completionPaymentUncapped;

            return this;
        }
    }
}

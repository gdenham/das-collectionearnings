using SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDeliveryPeriodisedValues;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Entities
{
    public class ProcessedLearningDeliveryPeriodisedValuesBuilder : IEntityBuilder<ProcessedLearningDeliveryPeriodisedValues>
    {
        private string _learnRefNumber = "Lrn001";
        private int _aimSeqNumber = 1;
        private string _attributeName = AttributeNames.OnProgrammePayment;
        private decimal _period1 = 1000m;
        private decimal _period2 = 1000m;
        private decimal _period3 = 1000m;
        private decimal _period4 = 1000m;
        private decimal _period5 = 1000m;
        private decimal _period6 = 1000m;
        private decimal _period7 = 1000m;
        private decimal _period8 = 1000m;
        private decimal _period9 = 1000m;
        private decimal _period10 = 1000m;
        private decimal _period11 = 1000m;
        private decimal _period12 = 1000m;

        public ProcessedLearningDeliveryPeriodisedValues Build()
        {
            return new ProcessedLearningDeliveryPeriodisedValues
            {
                LearnRefNumber = _learnRefNumber,
                AimSeqNumber = _aimSeqNumber,
                AttributeName = _attributeName,
                Period_1 = _period1,
                Period_2 = _period2,
                Period_3 = _period3,
                Period_4 = _period4,
                Period_5 = _period5,
                Period_6 = _period6,
                Period_7 = _period7,
                Period_8 = _period8,
                Period_9 = _period9,
                Period_10 = _period10,
                Period_11 = _period11,
                Period_12 = _period12
            };
        }

        public ProcessedLearningDeliveryPeriodisedValuesBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public ProcessedLearningDeliveryPeriodisedValuesBuilder WithAimSeqNumber(int aimseqNumber)
        {
            _aimSeqNumber = aimseqNumber;

            return this;
        }

        public ProcessedLearningDeliveryPeriodisedValuesBuilder WithAttributeName(string attributeName)
        {
            _attributeName = attributeName;

            return this;
        }

        public ProcessedLearningDeliveryPeriodisedValuesBuilder WithPeriodValue(decimal period)
        {
            _period1 = period;
            _period2 = period;
            _period3 = period;
            _period4 = period;
            _period5 = period;
            _period6 = period;
            _period7 = period;
            _period8 = period;
            _period9 = period;
            _period10 = period;
            _period11 = period;
            _period12 = period;

            return this;
        }
    }
}
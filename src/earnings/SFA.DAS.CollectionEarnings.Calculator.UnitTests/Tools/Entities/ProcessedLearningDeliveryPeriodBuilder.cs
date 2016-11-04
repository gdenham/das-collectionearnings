using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Entities
{
    public class ProcessedLearningDeliveryPeriodBuilder : IEntityBuilder<ProcessedLearningDeliveryPeriod>
    {
        private string _learnRefNumber = "Lrn001";
        private int _aimSeqNumber = 99;
        private int _period = 1;
        private decimal _programmeAimOnProgPayment = 1000m;
        private decimal _programmeAimCompletionPayment = 3000m;
        private decimal _programmeAimBalPayment = 2000m;

        public ProcessedLearningDeliveryPeriod Build()
        {
            return new ProcessedLearningDeliveryPeriod
            {
                LearnRefNumber = _learnRefNumber,
                AimSeqNumber = _aimSeqNumber,
                Period = _period,
                ProgrammeAimOnProgPayment = _programmeAimOnProgPayment,
                ProgrammeAimCompletionPayment = _programmeAimCompletionPayment,
                ProgrammeAimBalPayment = _programmeAimBalPayment
            };
        }

        public ProcessedLearningDeliveryPeriodBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public ProcessedLearningDeliveryPeriodBuilder WithAimSeqNumber(int aimseqNumber)
        {
            _aimSeqNumber = aimseqNumber;

            return this;
        }

        public ProcessedLearningDeliveryPeriodBuilder WithPeriod(int period)
        {
            _period = period;

            return this;
        }

        public ProcessedLearningDeliveryPeriodBuilder WithProgrammeAimOnProgPayment(decimal programmeAimOnProgPayment)
        {
            _programmeAimOnProgPayment = programmeAimOnProgPayment;

            return this;
        }

        public ProcessedLearningDeliveryPeriodBuilder WithProgrammeAimCompletionPayment(decimal programmeAimCompletionPayment)
        {
            _programmeAimCompletionPayment = programmeAimCompletionPayment;

            return this;
        }

        public ProcessedLearningDeliveryPeriodBuilder WithProgrammeAimBalPayment(decimal programmeAimBalPayment)
        {
            _programmeAimBalPayment = programmeAimBalPayment;

            return this;
        }
    }
}
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities
{
    public class ValidationErrorBuilder : IBuilder<ValidationError>
    {
        private string _learnRefNumber = "Lrn001";
        private long? _aimSeqNumber = 1;
        private string _ruleId = "DLOCK_01";

        public ValidationError Build()
        {
            return new ValidationError
            {
                LearnRefNumber = _learnRefNumber,
                AimSeqNumber = _aimSeqNumber,
                RuleId = _ruleId
            };
        }

        public ValidationErrorBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public ValidationErrorBuilder WithAimSeqNumber(long? aimseqNumber)
        {
            _aimSeqNumber = aimseqNumber;

            return this;
        }

        public ValidationErrorBuilder WithRuleId(string ruleId)
        {
            _ruleId = ruleId;

            return this;
        }
    }
}
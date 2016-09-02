using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities
{
    public class DasLearnerBuilder : IBuilder<DasLearner>
    {
        private long _ukprn = 10007459;
        private string _learnRefNumber = "Lrn001";
        private long? _uln = 1000000019;
        private string _niNumber = "AB123456C";
        private long? _aimSeqNumber = 1;
        private long? _stdCode;
        private long? _progType = 20;
        private long? _fworkCode = 550;
        private long? _pwayCode = 6;
        private long? _tbFinAmount = 15000;

        public DasLearner Build()
        {
            return new DasLearner
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
                TbFinAmount = _tbFinAmount
            };
        }

        public DasLearnerBuilder WithUkprn(long ukprn)
        {
            _ukprn = ukprn;

            return this;
        }

        public DasLearnerBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public DasLearnerBuilder WithUln(long? uln)
        {
            _uln = uln;

            return this;
        }

        public DasLearnerBuilder WithNiNumber(string niNumber)
        {
            _niNumber = niNumber;

            return this;
        }

        public DasLearnerBuilder WithAimSeqNumber(long? aimseqNumber)
        {
            _aimSeqNumber = aimseqNumber;

            return this;
        }

        public DasLearnerBuilder WithStdCode(long? stdCode)
        {
            _stdCode = stdCode;

            return this;
        }

        public DasLearnerBuilder WithProgType(long? progType)
        {
            _progType = progType;

            return this;
        }

        public DasLearnerBuilder WithFworkCode(long? fworkCode)
        {
            _fworkCode = fworkCode;

            return this;
        }

        public DasLearnerBuilder WithPwayCode(long? pwayCode)
        {
            _pwayCode = pwayCode;

            return this;
        }

        public DasLearnerBuilder WithTbFinAmount(long? tbFinAmount)
        {
            _tbFinAmount = tbFinAmount;

            return this;
        }
    }
}
using System;
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
        private long? _standardCode;
        private long? _programmeType = 20;
        private long? _frameworkCode = 550;
        private long? _pwayCode = 6;
        private long? _negotiatedPrice = 15000;
        private DateTime? _learnStartDate = new DateTime(2016, 9, 1);

        public DasLearner Build()
        {
            return new DasLearner
            {
                Ukprn = _ukprn,
                LearnRefNumber = _learnRefNumber,
                Uln = _uln,
                NiNumber = _niNumber,
                AimSeqNumber = _aimSeqNumber,
                SandardCode = _standardCode,
                ProgrammeType = _programmeType,
                FrameworkCode = _frameworkCode,
                PathwayCode = _pwayCode,
                NegotiatedPrice = _negotiatedPrice,
                LearnStartDate = _learnStartDate
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

        public DasLearnerBuilder WithStandardCode(long? standardCode)
        {
            _standardCode = standardCode;

            return this;
        }

        public DasLearnerBuilder WithProgrammeType(long? programmeType)
        {
            _programmeType = programmeType;

            return this;
        }

        public DasLearnerBuilder WithFrameworkCode(long? frameworkCode)
        {
            _frameworkCode = frameworkCode;

            return this;
        }

        public DasLearnerBuilder WithPathwayCode(long? pathwayCode)
        {
            _pwayCode = pathwayCode;

            return this;
        }

        public DasLearnerBuilder WithNegotiatedPrice(long? negotiatedPrice)
        {
            _negotiatedPrice = negotiatedPrice;

            return this;
        }

        public DasLearnerBuilder WithLearnStartDate(DateTime? learnStartDate)
        {
            _learnStartDate = learnStartDate;

            return this;
        }
    }
}
using System;
using SFA.DAS.CollectionEarnings.Calculator.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Entities
{
    public class LearningDeliveryToProcessBuilder : IEntityBuilder<LearningDeliveryToProcess>
    {
        private int _ukprn = 10007459;
        private string _learnRefNumber = "Lrn001";
        private long _uln = 1000000019;
        private string _niNumber = "AB123456C";
        private int _aimSeqNumber = 1;
        private long? _standardCode;
        private int? _programmeType = 20;
        private int? _frameworkCode = 550;
        private int? _pathwayCode = 6;
        private DateTime _learnStartDate = new DateTime(2017, 9, 1);
        private DateTime? _origLearnStartDate;
        private DateTime _learnPlanEndDate = new DateTime(2018, 9, 8);
        private DateTime? _learnActEndDate;
        private int? _completionStatus = 1;
        private int _negotiatedPrice = 15000;

        public LearningDeliveryToProcess Build()
        {
            return new LearningDeliveryToProcess
            {
                Ukprn = _ukprn,
                LearnRefNumber =  _learnRefNumber,
                Uln = _uln,
                NiNumber = _niNumber,
                AimSeqNumber = _aimSeqNumber,
                StandardCode = _standardCode,
                ProgrammeType = _programmeType,
                FrameworkCode = _frameworkCode,
                PathwayCode = _pathwayCode,
                LearnStartDate = _learnStartDate,
                OrigLearnStartDate = _origLearnStartDate,
                LearnPlanEndDate = _learnPlanEndDate,
                LearnActEndDate = _learnActEndDate,
                CompletionStatus = _completionStatus,
                NegotiatedPrice = _negotiatedPrice
            };
        }

        public LearningDeliveryToProcessBuilder WithUkprn(int ukprn)
        {
            _ukprn = ukprn;

            return this;
        }

        public LearningDeliveryToProcessBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public LearningDeliveryToProcessBuilder WithUln(long uln)
        {
            _uln = uln;

            return this;
        }

        public LearningDeliveryToProcessBuilder WithNiNumber(string niNumber)
        {
            _niNumber = niNumber;

            return this;
        }

        public LearningDeliveryToProcessBuilder WithAimSeqNumber(int aimseqNumber)
        {
            _aimSeqNumber = aimseqNumber;

            return this;
        }

        public LearningDeliveryToProcessBuilder WithStandardCode(long? standardCode)
        {
            _standardCode = standardCode;

            return this;
        }

        public LearningDeliveryToProcessBuilder WithProgrammeType(int? programmeType)
        {
            _programmeType = programmeType;

            return this;
        }

        public LearningDeliveryToProcessBuilder WithFrameworkCode(int? frameworkCode)
        {
            _frameworkCode = frameworkCode;

            return this;
        }

        public LearningDeliveryToProcessBuilder WithPathwayCode(int? pathwayCode)
        {
            _pathwayCode = pathwayCode;

            return this;
        }

        public LearningDeliveryToProcessBuilder WithLearnStartDate(DateTime learnStartDate)
        {
            _learnStartDate = learnStartDate;

            return this;
        }

        public LearningDeliveryToProcessBuilder WithOrigLearnStartDate(DateTime? origLearnStartDate)
        {
            _origLearnStartDate = origLearnStartDate;

            return this;
        }

        public LearningDeliveryToProcessBuilder WithLearnPlanEndDate(DateTime learnPlanEndDate)
        {
            _learnPlanEndDate = learnPlanEndDate;

            return this;
        }

        public LearningDeliveryToProcessBuilder WithLearnActEndDate(DateTime? learnActEndDate)
        {
            _learnActEndDate = learnActEndDate;

            return this;
        }

        public LearningDeliveryToProcessBuilder WithCompletionStatus(int? completionStatus)
        {
            _completionStatus = completionStatus;

            return this;
        }

        public LearningDeliveryToProcessBuilder WithNegotiatedPrice(int negotiatedPrice)
        {
            _negotiatedPrice = negotiatedPrice;

            return this;
        }
    }
}
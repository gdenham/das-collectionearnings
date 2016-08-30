using System;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Common.Tests.Data.Entities
{
    public class CommitmentBuilder : IBuilder<Commitment>
    {
        private string _commitmentId = "C-001";
        private long _uln = 1000000019;
        private long _ukprn = 10007459;
        private string _accountId = "A-001";
        private DateTime _startDate = new DateTime(2016, 9, 1);
        private DateTime _endDate = new DateTime(2018, 12, 31);
        private decimal _agreedCost = 15000.00m;
        private long? _standardCode;
        private int? _programmeType = 20;
        private int? _frameworkCode = 550;
        private int? _pathwayCode = 6;

        public Commitment Build()
        {
            return new Commitment
            {
                CommitmentId = _commitmentId,
                Uln = _uln,
                Ukprn = _ukprn,
                AccountId = _accountId,
                StartDate = _startDate,
                EndDate = _endDate,
                AgreedCost = _agreedCost,
                StandardCode = _standardCode,
                ProgrammeType = _programmeType,
                FrameworkCode = _frameworkCode,
                PathwayCode = _pathwayCode
            };
        }

        public CommitmentBuilder WithStandardCode(long? standardCode)
        {
            _standardCode = standardCode;

            return this;
        }

        public CommitmentBuilder WithProgrammeType(int? programmeType)
        {
            _programmeType = programmeType;

            return this;
        }

        public CommitmentBuilder WithFrameworkCode(int? frameworkCode)
        {
            _frameworkCode = frameworkCode;

            return this;
        }
        public CommitmentBuilder WithPathwayCode(int? pathwayCode)
        {
            _pathwayCode = pathwayCode;

            return this;
        }
    }
}
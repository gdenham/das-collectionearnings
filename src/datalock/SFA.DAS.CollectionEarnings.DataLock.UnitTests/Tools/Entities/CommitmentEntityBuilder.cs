﻿using System;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities
{
    public class CommitmentEntityBuilder : IBuilder<CommitmentEntity>
    {
        private long _commitmentId = 1;
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

        public CommitmentEntity Build()
        {
            return new CommitmentEntity
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

        public CommitmentEntityBuilder WithCommitmentId(long commitmentId)
        {
            _commitmentId = commitmentId;

            return this;
        }

        public CommitmentEntityBuilder WithUln(long uln)
        {
            _uln = uln;

            return this;
        }

        public CommitmentEntityBuilder Withukprn(long ukprn)
        {
            _ukprn = ukprn;

            return this;
        }

        public CommitmentEntityBuilder WithAccountId(string accountId)
        {
            _accountId = accountId;

            return this;
        }

        public CommitmentEntityBuilder WithStartDate(DateTime startDate)
        {
            _startDate = startDate;

            return this;
        }

        public CommitmentEntityBuilder WithEndDate(DateTime endDate)
        {
            _endDate = endDate;

            return this;
        }

        public CommitmentEntityBuilder WithAgreedCost(decimal agreedCost)
        {
            _agreedCost = agreedCost;

            return this;
        }

        public CommitmentEntityBuilder WithStandardCode(long? standardCode)
        {
            _standardCode = standardCode;

            return this;
        }

        public CommitmentEntityBuilder WithProgrammeType(int? programmeType)
        {
            _programmeType = programmeType;

            return this;
        }

        public CommitmentEntityBuilder WithFrameworkCode(int? frameworkCode)
        {
            _frameworkCode = frameworkCode;

            return this;
        }
        public CommitmentEntityBuilder WithPathwayCode(int? pathwayCode)
        {
            _pathwayCode = pathwayCode;

            return this;
        }
    }
}
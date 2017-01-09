﻿using System;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Enums;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application
{
    public class CommitmentBuilder : IBuilder<Commitment>
    {
        private long _commitmentId = 1;
        private long _versionId = 1;
        private long _uln = 1000000019;
        private long _ukprn = 10007459;
        private string _accountId = "A-001";
        private DateTime _startDate = new DateTime(2016, 9, 1);
        private DateTime _endDate = new DateTime(2018, 12, 31);
        private decimal _negoriatedPrice = 15000.00m;
        private long? _standardCode;
        private int? _programmeType = 20;
        private int? _frameworkCode = 550;
        private int? _pathwayCode = 6;
        private int _paymentStatus = 1;
        private string _paymentStatusDescription = "Active";
        private bool _payable = true;
        private DateTime _effectiveFrom = new DateTime(2016, 9, 1);
        private DateTime? _effectiveTo;

        public Commitment Build()
        {
            return new Commitment
            {
                CommitmentId = _commitmentId,
                VersionId = _versionId,
                Uln = _uln,
                Ukprn = _ukprn,
                AccountId = _accountId,
                StartDate = _startDate,
                EndDate = _endDate,
                NegotiatedPrice = _negoriatedPrice,
                StandardCode = _standardCode,
                ProgrammeType = _programmeType,
                FrameworkCode = _frameworkCode,
                PathwayCode = _pathwayCode,
                PaymentStatus = _paymentStatus,
                PaymentStatusDescription = _paymentStatusDescription,
                Payable = _payable,
                EffectiveFrom = _effectiveFrom,
                EffectiveTo = _effectiveTo
            };
        }

        public CommitmentBuilder WithCommitmentId(long commitmentId)
        {
            _commitmentId = commitmentId;

            return this;
        }

        public CommitmentBuilder WithVersionId(long versionId)
        {
            _versionId = versionId;

            return this;
        }

        public CommitmentBuilder WithUln(long uln)
        {
            _uln = uln;

            return this;
        }

        public CommitmentBuilder Withukprn(long ukprn)
        {
            _ukprn = ukprn;

            return this;
        }

        public CommitmentBuilder WithAccountId(string accountId)
        {
            _accountId = accountId;

            return this;
        }

        public CommitmentBuilder WithStartDate(DateTime startDate)
        {
            _startDate = startDate;

            return this;
        }

        public CommitmentBuilder WithEndDate(DateTime endDate)
        {
            _endDate = endDate;

            return this;
        }

        public CommitmentBuilder WithAgreedCost(decimal agreedCost)
        {
            _negoriatedPrice = agreedCost;

            return this;
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

        public CommitmentBuilder WithPaymentStatus(PaymentStatus paymentStatus)
        {
            _paymentStatus = (int)paymentStatus;
            _paymentStatusDescription = paymentStatus.ToString();
            _payable = paymentStatus == PaymentStatus.Active || paymentStatus == PaymentStatus.Completed;

            return this;
        }

        public CommitmentBuilder WithEffectiveFrom(DateTime effectiveFrom)
        {
            _effectiveFrom = effectiveFrom;

            return this;
        }

        public CommitmentBuilder WithEffectiveTo(DateTime? effectiveTo)
        {
            _effectiveTo = effectiveTo;

            return this;
        }
    }
}
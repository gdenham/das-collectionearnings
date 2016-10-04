﻿using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities
{
    public class LearnerCommitmentEntityBuilder : IBuilder<LearnerCommitmentEntity>
    {
        private long _ukprn = 10007459;
        private string _learnRefNumber = "Lrn001";
        private long _aimSeqNumber = 1;
        private string _commitmentId = "C-001";

        public LearnerCommitmentEntity Build()
        {
            return new LearnerCommitmentEntity
            {
                Ukprn = _ukprn,
                LearnRefNumber = _learnRefNumber,
                AimSeqNumber = _aimSeqNumber,
                CommitmentId = _commitmentId
            };
        }

        public LearnerCommitmentEntityBuilder WithUkprn(long ukprn)
        {
            _ukprn = ukprn;

            return this;
        }

        public LearnerCommitmentEntityBuilder WithLearnRefNumber(string learnRefNumber)
        {
            _learnRefNumber = learnRefNumber;

            return this;
        }

        public LearnerCommitmentEntityBuilder WithAimSeqNumber(long aimSeqNumber)
        {
            _aimSeqNumber = aimSeqNumber;

            return this;
        }

        public LearnerCommitmentEntityBuilder WithCommitmentId(string commitmentId)
        {
            _commitmentId = commitmentId;

            return this;
        }
    }
}
﻿using System;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.Commitment
{
    public class Commitment
    {
        public long CommitmentId { get; set; }
        public long VersionId { get; set; }

        public long Uln { get; set; }
        public long Ukprn { get; set; }
        public string AccountId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal NegotiatedPrice { get; set; }
        public long? StandardCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? FrameworkCode { get; set; }
        public int? PathwayCode { get; set; }

        public int PaymentStatus { get; set; }
        public string PaymentStatusDescription { get; set; }
        public bool Payable { get; set; }

        public int Priority { get; set; }

        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}
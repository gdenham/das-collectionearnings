using System;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities
{
    public class CommitmentEntity
    {
        public long CommitmentId { get; set; }
        public long Uln { get; set; }
        public long Ukprn { get; set; }
        public string AccountId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal AgreedCost { get; set; }
        public long? StandardCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? FrameworkCode { get; set; }
        public int? PathwayCode { get; set; }
    }
}
using System;

namespace SFA.DAS.CollectionEarnings.DataLock.Data.Entities
{
    public class DasLearner
    {
        public long Ukprn { get; set; }
        public string LearnRefNumber { get; set; }
        public long? Uln { get; set; }
        public string NiNumber { get; set; }
        public long? AimSeqNumber { get; set; }
        public long? StandardCode { get; set; }
        public long? ProgrammeType { get; set; }
        public long? FrameworkCode { get; set; }
        public long? PathwayCode { get; set; }
        public long? NegotiatedPrice { get; set; }
        public DateTime? LearnStartDate { get; set; }

        public const string SelectAll = "SELECT * FROM [DataLock].[vw_DasLearner]";
    }
}
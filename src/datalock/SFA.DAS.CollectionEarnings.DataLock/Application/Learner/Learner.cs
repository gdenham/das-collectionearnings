using System;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.Learner
{
    public class Learner
    {
        public long Ukprn { get; set; }
        public string LearnerReferenceNumber { get; set; }
        public long? Uln { get; set; }
        public string NiNumber { get; set; }
        public long? AimSequenceNumber { get; set; }
        public long? StandardCode { get; set; }
        public long? ProgrammeType { get; set; }
        public long? FrameworkCode { get; set; }
        public long? PathwayCode { get; set; }
        public long? NegotiatedPrice { get; set; }
        public DateTime LearningStartDate { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
    }
}
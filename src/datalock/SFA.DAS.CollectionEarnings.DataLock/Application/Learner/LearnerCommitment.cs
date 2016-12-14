namespace SFA.DAS.CollectionEarnings.DataLock.Application.Learner
{
    public class LearnerCommitment
    {
        public long Ukprn { get; set; }
        public string LearnerReferenceNumber { get; set; }
        public long AimSequenceNumber { get; set; }
        public long CommitmentId { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
    }
}
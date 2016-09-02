namespace SFA.DAS.CollectionEarnings.DataLock.Data.Entities
{
    public class DasLearner
    {
        public long Ukprn { get; set; }
        public string LearnRefNumber { get; set; }
        public long? Uln { get; set; }
        public string NiNumber { get; set; }
        public long? AimSeqNumber { get; set; }
        public long? StdCode { get; set; }
        public long? ProgType { get; set; }
        public long? FworkCode { get; set; }
        public long? PwayCode { get; set; }
        public long? TbFinAmount { get; set; }

        public const string SelectAll =
            @"
	        SELECT
		        (SELECT [UKPRN] FROM [Input].[LearningProvider]) AS [Ukprn],
		        l.[LearnRefNumber] AS [LearnRefNumber],
		        l.[ULN] AS [Uln],
		        l.[NINumber] AS [NiNumber],
		        ld.[AimSeqNumber] AS [AimSeqNumber],
		        ld.[StdCode] AS [StdCode],
		        ld.[ProgType] AS [ProgType],
		        ld.[FworkCode] AS [FworkCode],
		        ld.[PwayCode] AS [PwayCode],
		        afr.[TBFinAmount] AS [TbFinAmount]
	        FROM [Input].[Learner] l
		        JOIN [Input].[LearningDelivery] ld ON l.[Learner_Id] = ld.[Learner_Id]
		        JOIN [Input].[LearningDeliveryFAM] ldf ON ld.[LearningDelivery_Id] = ldf.[LearningDelivery_Id]
		        LEFT JOIN [Input].[TrailblazerApprenticeshipFinancialRecord] afr ON ld.[LearnRefNumber] = afr.[LearnRefNumber]
		           AND ld.[AimSeqNumber] = afr.[AimSeqNumber]
                   AND afr.[TBFinType] = 'TNP'
            WHERE ldf.[LearnDelFAMType] = 'ACT'
	           AND ldf.[LearnDelFAMCode] IN ('1', '3')";
    }
}
TRUNCATE TABLE [Reference].[Learners]
GO

INSERT INTO [Reference].[Learners]
    SELECT
		l.[Ukprn],
		l.[LearnRefNumber] AS [LearnRefNumber],
		l.[ULN] AS [Uln],
		l.[NINumber] AS [NiNumber],
		ld.[AimSeqNumber] AS [AimSeqNumber],
		ld.[StdCode] AS [StandardCode],
		ld.[ProgType] AS [ProgrammeType],
		ld.[FworkCode] AS [FrameworkCode],
		ld.[PwayCode] AS [PathwayCode],
		COALESCE(tnp1.[TBFinDate], tnp2.[TBFinDate], ld.[LearnStartDate]) AS [LearnStartDate],
		ISNULL(tnp1.[TBFinAmount],0) + ISNULL(tnp2.[TBFinAmount],0) AS [NegotiatedPrice]
	FROM ${ILR_Deds.FQ}.[Valid].[Learner] l
		JOIN ${ILR_Deds.FQ}.[Valid].[LearningDelivery] ld ON l.[Ukprn] = ld.[Ukprn]
            AND l.[LearnRefNumber] = ld.[LearnRefNumber]
		JOIN ${ILR_Deds.FQ}.[Valid].[LearningDeliveryFAM] ldf ON ld.[Ukprn] = ldf.[Ukprn]
            AND ld.[LearnRefNumber] = ldf.[LearnRefNumber]
            AND ld.[AimSeqNumber] = ldf.[AimSeqNumber]
		LEFT JOIN ${ILR_Deds.FQ}.[Valid].[TrailblazerApprenticeshipFinancialRecord] tnp1 
			ON ld.[Ukprn] = tnp1.[Ukprn]
            AND ld.[LearnRefNumber] = tnp1.[LearnRefNumber]
		    AND ld.[AimSeqNumber] = tnp1.[AimSeqNumber]
		    AND tnp1.[TBFinType] = 'TNP'
			AND tnp1.[TBFinCode] = 1
		LEFT JOIN ${ILR_Deds.FQ}.[Valid].[TrailblazerApprenticeshipFinancialRecord] tnp2 
			ON ld.[Ukprn] = tnp2.[Ukprn]
            AND ld.[LearnRefNumber] = tnp2.[LearnRefNumber]
		    AND ld.[AimSeqNumber] = tnp2.[AimSeqNumber]
		    AND tnp2.[TBFinType] = 'TNP'
		    AND tnp2.[TBFinCode] = 2
	WHERE ldf.[LearnDelFAMType] = 'ACT'
	   AND ldf.[LearnDelFAMCode] IN ('1', '3')
	UNION
    SELECT
		l.[Ukprn],
		l.[LearnRefNumber] AS [LearnRefNumber],
		l.[ULN] AS [Uln],
		l.[NINumber] AS [NiNumber],
		ld.[AimSeqNumber] AS [AimSeqNumber],
		ld.[StdCode] AS [StandardCode],
		ld.[ProgType] AS [ProgrammeType],
		ld.[FworkCode] AS [FrameworkCode],
		ld.[PwayCode] AS [PathwayCode],
		COALESCE(tnp3.[TBFinDate], tnp4.[TBFinDate], ld.[LearnStartDate]) AS [LearnStartDate],
		ISNULL(tnp3.[TBFinAmount],0) + ISNULL(tnp4.[TBFinAmount],0) AS [NegotiatedPrice]
	FROM ${ILR_Deds.FQ}.[Valid].[Learner] l
		JOIN ${ILR_Deds.FQ}.[Valid].[LearningDelivery] ld ON l.[Ukprn] = ld.[Ukprn]
            AND l.[LearnRefNumber] = ld.[LearnRefNumber]
		JOIN ${ILR_Deds.FQ}.[Valid].[LearningDeliveryFAM] ldf ON ld.[Ukprn] = ldf.[Ukprn]
            AND ld.[LearnRefNumber] = ldf.[LearnRefNumber]
            AND ld.[AimSeqNumber] = ldf.[AimSeqNumber]
		LEFT JOIN ${ILR_Deds.FQ}.[Valid].[TrailblazerApprenticeshipFinancialRecord] tnp3 
			ON ld.[Ukprn] = tnp3.[Ukprn]
            AND ld.[LearnRefNumber] = tnp3.[LearnRefNumber]
		    AND ld.[AimSeqNumber] = tnp3.[AimSeqNumber]
		    AND tnp3.[TBFinType] = 'TNP'
			AND tnp3.[TBFinCode] = 3
		LEFT JOIN ${ILR_Deds.FQ}.[Valid].[TrailblazerApprenticeshipFinancialRecord] tnp4 
			ON ld.[Ukprn] = tnp4.[Ukprn]
            AND ld.[LearnRefNumber] = tnp4.[LearnRefNumber]
		    AND ld.[AimSeqNumber] = tnp4.[AimSeqNumber]
		    AND tnp4.[TBFinType] = 'TNP'
		    AND tnp4.[TBFinCode] = 4
	WHERE ldf.[LearnDelFAMType] = 'ACT'
	   AND ldf.[LearnDelFAMCode] IN ('1', '3')
	   AND tnp3.[LearnRefNumber] IS NOT NULL
GO

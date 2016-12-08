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
		ape.[EpisodeStartDate] AS [LearnStartDate],
		ape.[PriceEpisodeTotalTNPPrice] AS [NegotiatedPrice],
		ape.[PriceEpisodeIdentifier]
	FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode] ape
		JOIN ${ILR_Deds.FQ}.[Valid].[Learner] l ON ape.[Ukprn] = l.[Ukprn]
			AND ape.[LearnRefNumber] = l.[LearnRefNumber]
		JOIN ${ILR_Deds.FQ}.[Valid].[LearningDelivery] ld ON ape.[Ukprn] = ld.[Ukprn]
			AND ape.[LearnRefNumber] = ld.[LearnRefNumber]
			AND ape.[AimSeqNumber] = ld.[AimSeqNumber]
		JOIN ${ILR_Deds.FQ}.[Valid].[LearningDeliveryFAM] ldf ON ld.[Ukprn] = ldf.[Ukprn]
			AND ld.[LearnRefNumber] = ldf.[LearnRefNumber]
			AND ld.[AimSeqNumber] = ldf.[AimSeqNumber]
	WHERE ldf.[LearnDelFAMType] = 'ACT'
		AND ldf.[LearnDelFAMCode] IN ('1', '3')
GO

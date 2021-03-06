IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='DataLock')
BEGIN
	EXEC('CREATE SCHEMA DataLock')
END

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_DasLearner
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_DasLearner' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
	DROP VIEW DataLock.vw_DasLearner
END
GO

CREATE VIEW DataLock.vw_DasLearner
AS 
	SELECT
		(SELECT [UKPRN] FROM [Valid].[LearningProvider]) AS [Ukprn],
		l.[LearnRefNumber] AS [LearnRefNumber],
		l.[ULN] AS [Uln],
		l.[NINumber] AS [NiNumber],
		ld.[AimSeqNumber] AS [AimSeqNumber],
		ld.[StdCode] AS [StandardCode],
		ld.[ProgType] AS [ProgrammeType],
		ld.[FworkCode] AS [FrameworkCode],
		ld.[PwayCode] AS [PathwayCode],
		ape.[EpisodeEffectiveTNPStartDate] AS [LearnStartDate],
		ape.[PriceEpisodeTotalTNPPrice] AS [NegotiatedPrice],
		ape.[PriceEpisodeIdentifier]
	FROM [Rulebase].[AEC_ApprenticeshipPriceEpisode] ape
	   JOIN [Valid].[Learner] l ON ape.[LearnRefNumber] = l.[LearnRefNumber]
	   JOIN [Valid].[LearningDelivery] ld ON ape.[LearnRefNumber] = ld.[LearnRefNumber]
		  AND ape.[PriceEpisodeAimSeqNumber] = ld.[AimSeqNumber]
	   JOIN [Valid].[LearningDeliveryFAM] ldf ON ld.[LearnRefNumber] = ldf.[LearnRefNumber]
		  AND ld.[AimSeqNumber] = ldf.[AimSeqNumber]
	WHERE ldf.[LearnDelFAMType] = 'ACT'
        AND ldf.[LearnDelFAMCode] = '1'
        AND ldf.[LearnDelFAMDateFrom] <= ape.[EpisodeEffectiveTNPStartDate]
        AND ldf.[LearnDelFAMDateTo] >= COALESCE(ape.[PriceEpisodeActualEndDate], ape.[PriceEpisodePlannedEndDate], ld.[LearnActEndDate], ld.[LearnPlanEndDate])
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_Providers
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_Providers' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
	DROP VIEW DataLock.vw_Providers
END
GO

CREATE VIEW DataLock.vw_Providers
AS
SELECT
	p.UKPRN
FROM Valid.LearningProvider p
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_ValidationError
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_ValidationError' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
	DROP VIEW DataLock.vw_ValidationError
END
GO

CREATE VIEW DataLock.vw_ValidationError
AS
SELECT 
	ve.[Ukprn],
	ve.[LearnRefNumber],
	ve.[AimSeqNumber],
	ve.[RuleId],
	ve.[PriceEpisodeIdentifier],
	cp.[CollectionPeriodName],
	cp.[CollectionPeriodMonth],
	cp.[CollectionPeriodYear]
FROM DataLock.ValidationError ve
    CROSS JOIN (
	   SELECT TOP 1
		  '${YearOfCollection}-' + [Name] AS [CollectionPeriodName],
		  [CalendarMonth] AS [CollectionPeriodMonth],
		  [CalendarYear] AS [CollectionPeriodYear]
	   FROM [Reference].[CollectionPeriods]
	   WHERE [Open] = 1
    ) cp
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_DasLearnerCommitment
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_DasLearnerCommitment' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
	DROP VIEW DataLock.vw_DasLearnerCommitment
END
GO

CREATE VIEW DataLock.vw_DasLearnerCommitment
AS
SELECT 
	dlc.[Ukprn],
	dlc.[LearnRefNumber],
	dlc.[AimSeqNumber],
	dlc.[CommitmentId],
	dlc.[PriceEpisodeIdentifier],
	cp.[CollectionPeriodName],
	cp.[CollectionPeriodMonth],
	cp.[CollectionPeriodYear]
FROM DataLock.DasLearnerCommitment dlc
    CROSS JOIN (
	   SELECT TOP 1
		  '${YearOfCollection}-' + [Name] AS [CollectionPeriodName],
		  [CalendarMonth] AS [CollectionPeriodMonth],
		  [CalendarYear] AS [CollectionPeriodYear]
	   FROM [Reference].[CollectionPeriods]
	   WHERE [Open] = 1
    ) cp
GO
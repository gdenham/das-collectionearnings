IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Rulebase')
BEGIN
	EXEC('CREATE SCHEMA Rulebase')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_AEC_LearnersToProcess: Learning Deliveries that need to be picked up by the Apprenticeship Earnings funding calculator
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_AEC_LearnersToProcess' AND [schema_id] = SCHEMA_ID('Rulebase'))
BEGIN
	DROP VIEW [Rulebase].[vw_AEC_LearnersToProcess]
END
GO

CREATE VIEW [Rulebase].[vw_AEC_LearnersToProcess]
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
	ld.[LearnStartDate] AS [LearnStartDate],
	ld.[OrigLearnStartDate] AS [OrigLearnStartDate],
	ld.[LearnPlanEndDate] AS [LearnPlanEndDate],
	ld.[LearnActEndDate] AS [LearnActEndDate],
	ld.[CompStatus] AS [CompletionStatus]
FROM [Valid].[Learner] l
	JOIN [Valid].[LearningDelivery] ld 
		ON ld.[LearnRefNumber] = l.[LearnRefNumber]
WHERE ld.[FundModel] = 36
	AND ld.[LearnAimRef] = 'ZPROG001'
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_AEC_ApprenticeshipPriceEpisode
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_AEC_ApprenticeshipPriceEpisode' AND [schema_id] = SCHEMA_ID('Rulebase'))
BEGIN
	DROP VIEW [Rulebase].[vw_AEC_ApprenticeshipPriceEpisode]
END
GO

CREATE VIEW [Rulebase].[vw_AEC_ApprenticeshipPriceEpisode]
AS
	SELECT
		(SELECT [UKPRN] FROM [Valid].[LearningProvider]) AS [Ukprn],
		[LearnRefNumber],
		[PriceEpisodeIdentifier],
		[EpisodeEffectiveTNPStartDate],
		[EpisodeStartDate],
		[PriceEpisodeActualEndDate],
		[PriceEpisodeActualInstalments],
		[PriceEpisodeAimSeqNumber],
		[PriceEpisodeCappedRemainingTNPAmount],
		[PriceEpisodeCompleted],
		[PriceEpisodeCompletionElement],
		[PriceEpisodeExpectedTotalMonthlyValue],
		[PriceEpisodeInstalmentValue],
		[PriceEpisodePlannedEndDate],
		[PriceEpisodePlannedInstalments],
		[PriceEpisodePreviousEarnings],
		[PriceEpisodeRemainingAmountWithinUpperLimit],
		[PriceEpisodeRemainingTNPAmount],
		[PriceEpisodeTotalEarnings],
		[PriceEpisodeTotalTNPPrice],
		[PriceEpisodeUpperBandLimit],
		[PriceEpisodeUpperLimitAdjustment],
		[TNP1],
		[TNP2],
		[TNP3],
		[TNP4]
	FROM [Rulebase].[AEC_ApprenticeshipPriceEpisode]
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_AEC_ApprenticeshipPriceEpisode_Period
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_AEC_ApprenticeshipPriceEpisode_Period' AND [schema_id] = SCHEMA_ID('Rulebase'))
BEGIN
	DROP VIEW [Rulebase].[vw_AEC_ApprenticeshipPriceEpisode_Period]
END
GO

CREATE VIEW [Rulebase].[vw_AEC_ApprenticeshipPriceEpisode_Period]
AS
	SELECT
		(SELECT [UKPRN] FROM [Valid].[LearningProvider]) AS [Ukprn],
		[LearnRefNumber],
		[PriceEpisodeIdentifier],
		[Period],
		[PriceEpisodeApplic1618FrameworkUpliftBalancing],
		[PriceEpisodeApplic1618FrameworkUpliftCompletionPayment],
		[PriceEpisodeApplic1618FrameworkUpliftOnProgPayment],
		[PriceEpisodeBalancePayment],
		[PriceEpisodeBalanceValue],
		[PriceEpisodeCompletionPayment],
		[PriceEpisodeFirstDisadvantagePayment],
		[PriceEpisodeFirstEmp1618Pay],
		[PriceEpisodeFirstProv1618Pay],
		[PriceEpisodeFundLineType],
		[PriceEpisodeInstalmentsThisPeriod],
		[PriceEpisodeLSFCash],
		[PriceEpisodeOnProgPayment],
		[PriceEpisodeSecondDisadvantagePayment],
		[PriceEpisodeSecondEmp1618Pay],
		[PriceEpisodeSecondProv1618Pay]
	FROM [Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_AEC_ApprenticeshipPriceEpisode_PeriodisedValues
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_AEC_ApprenticeshipPriceEpisode_PeriodisedValues' AND [schema_id] = SCHEMA_ID('Rulebase'))
BEGIN
	DROP VIEW [Rulebase].[vw_AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
END
GO

CREATE VIEW [Rulebase].[vw_AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
AS
	SELECT
		(SELECT [UKPRN] FROM [Valid].[LearningProvider]) AS [Ukprn],
		[LearnRefNumber],
		[PriceEpisodeIdentifier],
		[AttributeName],
		[Period_1],
		[Period_2],
		[Period_3],
		[Period_4],
		[Period_5],
		[Period_6],
		[Period_7],
		[Period_8],
		[Period_9],
		[Period_10],
		[Period_11],
		[Period_12]
	FROM [Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_AEC_ApprenticeshipPriceEpisode_PeriodisedTextValues
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_AEC_ApprenticeshipPriceEpisode_PeriodisedTextValues' AND [schema_id] = SCHEMA_ID('Rulebase'))
BEGIN
	DROP VIEW [Rulebase].[vw_AEC_ApprenticeshipPriceEpisode_PeriodisedTextValues]
END
GO

CREATE VIEW [Rulebase].[vw_AEC_ApprenticeshipPriceEpisode_PeriodisedTextValues]
AS
	SELECT
		(SELECT [UKPRN] FROM [Valid].[LearningProvider]) AS [Ukprn],
		[LearnRefNumber],
		[PriceEpisodeIdentifier],
		[AttributeName],
		[Period_1],
		[Period_2],
		[Period_3],
		[Period_4],
		[Period_5],
		[Period_6],
		[Period_7],
		[Period_8],
		[Period_9],
		[Period_10],
		[Period_11],
		[Period_12]
	FROM [Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
GO
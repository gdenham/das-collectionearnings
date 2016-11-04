IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Rulebase')
BEGIN
	EXEC('CREATE SCHEMA Rulebase')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_AE_LearningDeliveriesToProcess: Learning Deliveries that need to be picked up by the Apprenticeship Earnings funding calculator
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_AE_LearningDeliveriesToProcess' AND [schema_id] = SCHEMA_ID('Rulebase'))
BEGIN
	DROP VIEW [Rulebase].[vw_AE_LearningDeliveriesToProcess]
END
GO

CREATE VIEW [Rulebase].[vw_AE_LearningDeliveriesToProcess]
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
	ld.[CompStatus] AS [CompletionStatus],
	ISNULL(tnp1.[TBFinAmount],0) + ISNULL(tnp2.[TBFinAmount],0) AS [NegotiatedPrice]
FROM [Valid].[Learner] l
	JOIN [Valid].[LearningDelivery] ld 
		ON ld.[LearnRefNumber] = l.[LearnRefNumber]
	LEFT JOIN [Valid].[TrailblazerApprenticeshipFinancialRecord] tnp1 
		ON tnp1.[LearnRefNumber] = ld.[LearnRefNumber]
		AND tnp1.[AimSeqNumber] = ld.[AimSeqNumber]
		AND tnp1.[TBFinType] = 'TNP'
		AND tnp1.TBFinCode = 1
	LEFT JOIN [Valid].[TrailblazerApprenticeshipFinancialRecord] tnp2 
		ON tnp2.[LearnRefNumber] = ld.[LearnRefNumber]
		AND tnp2.[AimSeqNumber] = ld.[AimSeqNumber]
		AND tnp2.[TBFinType] = 'TNP'
		AND tnp2.TBFinCode = 2
WHERE ld.[FundModel] = 36
	AND ld.[LearnAimRef] = 'ZPROG001'
	AND ISNULL(tnp1.[TBFinAmount],0) + ISNULL(tnp2.[TBFinAmount],0) > 0
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_AE_LearningDelivery_PeriodisedValues
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_AE_LearningDelivery_PeriodisedValues' AND [schema_id] = SCHEMA_ID('Rulebase'))
BEGIN
	DROP VIEW [Rulebase].[vw_AE_LearningDelivery_PeriodisedValues]
END
GO

CREATE VIEW [Rulebase].[vw_AE_LearningDelivery_PeriodisedValues]
AS
	SELECT
		(SELECT [UKPRN] FROM [Valid].[LearningProvider]) AS [Ukprn],
		[LearnRefNumber],
		[AimSeqNumber],
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
	FROM [Rulebase].[AE_LearningDelivery_PeriodisedValues]
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_AE_LearningDelivery_Period
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_AE_LearningDelivery_Period' AND [schema_id] = SCHEMA_ID('Rulebase'))
BEGIN
	DROP VIEW [Rulebase].[vw_AE_LearningDelivery_Period]
END
GO

CREATE VIEW [Rulebase].[vw_AE_LearningDelivery_Period]
AS
	SELECT
		(SELECT [UKPRN] FROM [Valid].[LearningProvider]) AS [Ukprn],
		[LearnRefNumber],
		[AimSeqNumber],
		[Period],
		[ProgrammeAimOnProgPayment],
		[ProgrammeAimCompletionPayment],
		[ProgrammeAimBalPayment]
	FROM [Rulebase].[AE_LearningDelivery_Period]
GO

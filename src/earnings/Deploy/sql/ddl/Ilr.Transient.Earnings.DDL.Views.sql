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
		tafr.[TBFinAmount] AS [NegotiatedPrice]
	FROM [Valid].[Learner] l
		JOIN [Valid].[LearningDelivery] ld ON ld.[LearnRefNumber] = l.[LearnRefNumber]
		JOIN [Valid].[TrailblazerApprenticeshipFinancialRecord] tafr ON tafr.[LearnRefNumber] = ld.[LearnRefNumber]
			AND tafr.[AimSeqNumber] = ld.[AimSeqNumber]
	WHERE ld.[FundModel] = 36
		AND ld.[LearnAimRef] = 'ZPROG001'
		AND tafr.[TBFinType] = 'TNP'
GO

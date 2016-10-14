IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='DataLock')
BEGIN
	EXEC('CREATE SCHEMA DataLock')
END
GO

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
		l.[Ukprn],
		l.[LearnRefNumber] AS [LearnRefNumber],
		l.[ULN] AS [Uln],
		l.[NINumber] AS [NiNumber],
		ld.[AimSeqNumber] AS [AimSeqNumber],
		ld.[StdCode] AS [StandardCode],
		ld.[ProgType] AS [ProgrammeType],
		ld.[FworkCode] AS [FrameworkCode],
		ld.[PwayCode] AS [PathwayCode],
		ld.[LearnStartDate] AS [LearnStartDate],
		afr.[TBFinAmount] AS [NegotiatedPrice]
	FROM ${ILR_Current.FQ}.[Valid].[Learner] l
		JOIN ${ILR_Current.FQ}.[Valid].[LearningDelivery] ld ON l.[Ukprn] = ld.[Ukprn]
            AND l.[LearnRefNumber] = ld.[LearnRefNumber]
		JOIN ${ILR_Current.FQ}.[Valid].[LearningDeliveryFAM] ldf ON ld.[Ukprn] = ldf.[Ukprn]
            AND ld.[LearnRefNumber] = ldf.[LearnRefNumber]
            AND ld.[AimSeqNumber] = ldf.[AimSeqNumber]
		LEFT JOIN ${ILR_Current.FQ}.[Valid].[TrailblazerApprenticeshipFinancialRecord] afr ON ld.[Ukprn] = afr.[Ukprn]
           AND ld.[LearnRefNumber] = afr.[LearnRefNumber]
		   AND ld.[AimSeqNumber] = afr.[AimSeqNumber]
		   AND afr.[TBFinType] = 'TNP'
	WHERE ldf.[LearnDelFAMType] = 'ACT'
	   AND ldf.[LearnDelFAMCode] IN ('1', '3')
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
FROM ${ILR_Current.FQ}.Valid.LearningProvider p
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
    [Ukprn],
    [LearnRefNumber],
    [AimSeqNumber],
    [RuleId],
    (SELECT MAX([Collection_Period]) FROM ${ILR_Summarisation.FQ}.dbo.Collection_Period_Mapping WHERE [Collection_Open] = 1) AS [CollectionPeriodName],
    (SELECT MAX([Period]) FROM ${ILR_Summarisation.FQ}.dbo.Collection_Period_Mapping WHERE [Collection_Open] = 1) AS [CollectionPeriodMonth],
    (SELECT MAX([Calendar_Year]) FROM ${ILR_Summarisation.FQ}.dbo.Collection_Period_Mapping WHERE [Collection_Open] = 1) AS [CollectionPeriodYear]
FROM DataLock.ValidationError
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
    [Ukprn],
    [LearnRefNumber],
    [AimSeqNumber],
    [CommitmentId],
    (SELECT MAX([Collection_Period]) FROM ${ILR_Summarisation.FQ}.dbo.Collection_Period_Mapping WHERE [Collection_Open] = 1) AS [CollectionPeriodName],
    (SELECT MAX([Period]) FROM ${ILR_Summarisation.FQ}.dbo.Collection_Period_Mapping WHERE [Collection_Open] = 1) AS [CollectionPeriodMonth],
    (SELECT MAX([Calendar_Year]) FROM ${ILR_Summarisation.FQ}.dbo.Collection_Period_Mapping WHERE [Collection_Open] = 1) AS [CollectionPeriodYear]
FROM DataLock.DasLearnerCommitment
GO
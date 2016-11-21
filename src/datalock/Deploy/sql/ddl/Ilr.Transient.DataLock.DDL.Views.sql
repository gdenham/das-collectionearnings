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
		(SELECT [UKPRN] FROM [Input].[LearningProvider]) AS [Ukprn],
		l.[LearnRefNumber] AS [LearnRefNumber],
		l.[ULN] AS [Uln],
		l.[NINumber] AS [NiNumber],
		ld.[AimSeqNumber] AS [AimSeqNumber],
		ld.[StdCode] AS [StandardCode],
		ld.[ProgType] AS [ProgrammeType],
		ld.[FworkCode] AS [FrameworkCode],
		ld.[PwayCode] AS [PathwayCode],
		ld.[LearnStartDate] AS [LearnStartDate],
		ISNULL(tnp1.[TBFinAmount],0) + ISNULL(tnp2.[TBFinAmount],0) AS [NegotiatedPrice]
	FROM [Input].[Learner] l
		JOIN [Input].[LearningDelivery] ld ON l.[Learner_Id] = ld.[Learner_Id]
		JOIN [Input].[LearningDeliveryFAM] ldf ON ld.[LearningDelivery_Id] = ldf.[LearningDelivery_Id]
		LEFT JOIN [Input].[TrailblazerApprenticeshipFinancialRecord] tnp1 
			ON ld.[LearnRefNumber] = tnp1.[LearnRefNumber]
		    AND ld.[AimSeqNumber] = tnp1.[AimSeqNumber]
		    AND tnp1.[TBFinType] = 'TNP'
			AND tnp1.[TBFinCode] = 1
		LEFT JOIN [Input].[TrailblazerApprenticeshipFinancialRecord] tnp2 
			ON ld.[LearnRefNumber] = tnp2.[LearnRefNumber]
		    AND ld.[AimSeqNumber] = tnp2.[AimSeqNumber]
		    AND tnp2.[TBFinType] = 'TNP'
			AND tnp2.[TBFinCode] = 2
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
FROM Input.LearningProvider p
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
    (SELECT MAX('${YearOfCollection}-' + [Name]) FROM Reference.CollectionPeriods WHERE [Open] = 1) AS [CollectionPeriodName],
    (SELECT MAX([CalendarMonth]) FROM Reference.CollectionPeriods WHERE [Open] = 1) AS [CollectionPeriodMonth],
    (SELECT MAX([CalendarYear]) FROM Reference.CollectionPeriods WHERE [Open] = 1) AS [CollectionPeriodYear]
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
    (SELECT MAX('${YearOfCollection}-' + [Name]) FROM Reference.CollectionPeriods WHERE [Open] = 1) AS [CollectionPeriodName],
    (SELECT MAX([CalendarMonth]) FROM Reference.CollectionPeriods WHERE [Open] = 1) AS [CollectionPeriodMonth],
    (SELECT MAX([CalendarYear]) FROM Reference.CollectionPeriods WHERE [Open] = 1) AS [CollectionPeriodYear]
FROM DataLock.DasLearnerCommitment
GO
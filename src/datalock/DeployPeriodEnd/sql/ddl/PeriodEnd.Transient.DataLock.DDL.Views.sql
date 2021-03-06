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
		[Ukprn],
		[LearnRefNumber],
		[Uln],
		[NiNumber],
		[AimSeqNumber],
		[StandardCode],
		[ProgrammeType],
		[FrameworkCode],
		[PathwayCode],
		[LearnStartDate],
		[NegotiatedPrice],
		[PriceEpisodeIdentifier]
	FROM Reference.Learners
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
	p.Ukprn
FROM Reference.Providers p
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
IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='DataLock')
BEGIN
	EXEC('CREATE SCHEMA DataLock')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- ValidationError
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ValidationError' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
	DROP TABLE DataLock.ValidationError
END
GO

CREATE TABLE DataLock.ValidationError
(
    [Ukprn] bigint,
    [LearnRefNumber] varchar(100),
    [AimSeqNumber] bigint,
    [RuleId] varchar(50),
	[PriceEpisodeIdentifier] varchar(25) NOT NULL,
    [CollectionPeriodName] varchar(8) NOT NULL,
    [CollectionPeriodMonth] int NOT NULL,
    [CollectionPeriodYear] int NOT NULL
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- DasLearnerCommitment
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='DasLearnerCommitment' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
	DROP TABLE DataLock.DasLearnerCommitment
END
GO

CREATE TABLE DataLock.DasLearnerCommitment
(
	[Ukprn] bigint NOT NULL,
	[LearnRefNumber] varchar(100) NOT NULL,
	[AimSeqNumber] bigint NOT NULL,
	[CommitmentId] varchar(50) NOT NULL,
	[PriceEpisodeIdentifier] varchar(25) NOT NULL,
    [CollectionPeriodName] varchar(8) NOT NULL,
    [CollectionPeriodMonth] int NOT NULL,
    [CollectionPeriodYear] int NOT NULL
)
GO

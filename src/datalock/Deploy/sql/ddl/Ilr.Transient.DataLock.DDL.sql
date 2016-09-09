IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='DataLock')
BEGIN
	EXEC('CREATE SCHEMA DataLock')
END

-----------------------------------------------------------------------------------------------------------------------------------------------
-- TaskLog
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='TaskLog' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
	DROP TABLE DataLock.TaskLog
END
GO

CREATE TABLE DataLock.TaskLog
(
	[TaskLogId] uniqueidentifier NOT NULL DEFAULT(NEWID()),
	[DateTime] datetime NOT NULL DEFAULT(GETDATE()),
	[Level] nvarchar(10) NOT NULL,
	[Logger] nvarchar(512) NOT NULL,
	[Message] nvarchar(1024) NOT NULL,
	[Exception] nvarchar(max) NULL
)

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
	[LearnRefNumber] varchar(100),
	[AimSeqNumber] bigint,
	[RuleId] varchar(50)
)

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
		afr.[TBFinAmount] AS [NegotiatedPrice]
	FROM [Input].[Learner] l
		JOIN [Input].[LearningDelivery] ld ON l.[Learner_Id] = ld.[Learner_Id]
		JOIN [Input].[LearningDeliveryFAM] ldf ON ld.[LearningDelivery_Id] = ldf.[LearningDelivery_Id]
		LEFT JOIN [Input].[TrailblazerApprenticeshipFinancialRecord] afr ON ld.[LearnRefNumber] = afr.[LearnRefNumber]
		   AND ld.[AimSeqNumber] = afr.[AimSeqNumber]
		   AND afr.[TBFinType] = 'TNP'
	WHERE ldf.[LearnDelFAMType] = 'ACT'
	   AND ldf.[LearnDelFAMCode] IN ('1', '3')
GO

IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Reference')
BEGIN
	EXEC('CREATE SCHEMA Reference')
END

-----------------------------------------------------------------------------------------------------------------------------------------------
-- DataLockCommitments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='DataLockCommitments' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.DataLockCommitments
END
GO

CREATE TABLE Reference.DataLockCommitments
(
	[CommitmentId] varchar(50) PRIMARY KEY,
	[Uln] bigint NOT NULL,
	[Ukprn] bigint NOT NULL,
	[AccountId] varchar(50) NOT NULL,
	[StartDate] date NOT NULL,
	[EndDate] date NOT NULL,
	[AgreedCost] decimal(15, 2) NOT NULL,
	[StandardCode] bigint NULL,
	[ProgrammeType] int NULL,
	[FrameworkCode] int NULL,
	[PathwayCode] int NULL
)
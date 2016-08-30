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
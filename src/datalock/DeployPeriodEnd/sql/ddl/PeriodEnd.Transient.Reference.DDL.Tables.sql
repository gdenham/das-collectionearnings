IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Reference')
BEGIN
	EXEC('CREATE SCHEMA Reference')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- DasCommitments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='DasCommitments' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.DasCommitments
END
GO

CREATE TABLE Reference.DasCommitments
(
	[CommitmentId] bigint PRIMARY KEY,
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
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- CollectionPeriods
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='CollectionPeriods' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.CollectionPeriods
END
GO

CREATE TABLE Reference.CollectionPeriods (
	[Id] int NOT NULL,
	[Name] varchar(3) NOT NULL,
	[CalendarMonth] int NOT NULL,
	[CalendarYear] int NOT NULL,
	[Open] bit NOT NULL,
	CONSTRAINT [PK_CollectionPeriods] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Providers
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Providers' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.Providers
END
GO

CREATE TABLE Reference.Providers (
	[Ukprn] bigint NOT NULL,
	CONSTRAINT [PK_Providers] PRIMARY KEY CLUSTERED ([Ukprn] ASC)
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Learners
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Learners' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.Learners
END
GO

CREATE TABLE Reference.Learners(
	[Ukprn] bigint NOT NULL,
	[LearnRefNumber] varchar(12) NOT NULL,
	[Uln] bigint NOT NULL,
	[NiNumber] varchar(9) NULL,
	[AimSeqNumber] int NOT NULL,
	[StandardCode] bigint NULL,
	[ProgrammeType] int NULL,
	[FrameworkCode] int NULL,
	[PathwayCode] int NULL,
	[LearnStartDate] date NOT NULL,
	[NegotiatedPrice] int NOT NULL
)
GO

CREATE INDEX [IDX_Learners_Ukprn] ON Reference.Learners ([Ukprn])
GO
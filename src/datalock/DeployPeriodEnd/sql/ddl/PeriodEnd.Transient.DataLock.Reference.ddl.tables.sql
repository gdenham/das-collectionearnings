IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Reference')
BEGIN
	EXEC('CREATE SCHEMA Reference')
END
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
	[NegotiatedPrice] int NOT NULL,
	[PriceEpisodeIdentifier] varchar(25) NOT NULL
)
GO

CREATE CLUSTERED INDEX [IDX_Learners_Ukprn] ON Reference.Learners ([Ukprn])
GO
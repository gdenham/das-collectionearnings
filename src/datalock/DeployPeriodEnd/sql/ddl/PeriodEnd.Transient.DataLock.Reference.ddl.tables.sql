IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Reference')
BEGIN
	EXEC('CREATE SCHEMA Reference')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- DataLockPriceEpisode
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='DataLockPriceEpisode' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.DataLockPriceEpisode
END
GO

CREATE TABLE Reference.DataLockPriceEpisode (
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

CREATE CLUSTERED INDEX [IDX_DataLockPriceEpisode_Ukprn] ON Reference.DataLockPriceEpisode ([Ukprn])
GO
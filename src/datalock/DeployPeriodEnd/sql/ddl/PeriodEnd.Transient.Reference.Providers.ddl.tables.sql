IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Reference')
BEGIN
	EXEC('CREATE SCHEMA Reference')
END
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
	[IlrSubmissionDateTime] datetime NOT NULL,
	CONSTRAINT [PK_Providers] PRIMARY KEY CLUSTERED (
        [Ukprn] ASC
    )
)
GO

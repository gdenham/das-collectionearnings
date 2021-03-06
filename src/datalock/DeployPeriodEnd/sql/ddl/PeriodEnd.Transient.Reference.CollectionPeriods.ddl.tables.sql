IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Reference')
BEGIN
	EXEC('CREATE SCHEMA Reference')
END
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
	CONSTRAINT [PK_CollectionPeriods] PRIMARY KEY CLUSTERED (
        [Id] ASC
    )
)
GO

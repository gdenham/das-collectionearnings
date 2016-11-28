-----------------------------------------------------------------------------------------------------------------------------------------------
-- Commitments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='DasCommitments' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE [dbo].[DasCommitments]
END
GO

CREATE TABLE [dbo].[DasCommitments](
	[CommitmentId] [bigint] NOT NULL,
	[Uln] [bigint] NOT NULL,
	[Ukprn] [bigint] NOT NULL,
	[AccountId] [varchar](50) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[AgreedCost] [decimal](15, 2) NOT NULL,
	[StandardCode] [bigint] NULL,
	[ProgrammeType] [int] NULL,
	[FrameworkCode] [int] NULL,
	[PathwayCode] [int] NULL,
	[PaymentStatus] [int] NOT NULL,
	[PaymentStatusDescription] [varchar](50) NOT NULL,
	[Payable] [bit] NOT NULL,
	[Priority] [int] NOT NULL,
	[VersionId] [varchar](50) NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[CommitmentId] ASC
	)
)

-----------------------------------------------------------------------------------------------------------------------------------------------
-- EventStreamPointer
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='EventStreamPointer' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE [dbo].[EventStreamPointer]
END
GO

CREATE TABLE [dbo].[EventStreamPointer](
	[EventId] [bigint] NOT NULL,
	[ReadDate] [datetime] NOT NULL
)
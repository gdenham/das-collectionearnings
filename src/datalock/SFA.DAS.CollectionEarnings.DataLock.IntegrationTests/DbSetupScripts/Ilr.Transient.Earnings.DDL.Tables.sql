IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Earnings')
BEGIN
	EXEC('CREATE SCHEMA Earnings')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- TaskLog
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='TaskLog' AND [schema_id] = SCHEMA_ID('Earnings'))
BEGIN
	DROP TABLE Earnings.TaskLog
END
GO

CREATE TABLE Earnings.TaskLog
(
	[TaskLogId] uniqueidentifier NOT NULL DEFAULT(NEWID()),
	[DateTime] datetime NOT NULL DEFAULT(GETDATE()),
	[Level] nvarchar(10) NOT NULL,
	[Logger] nvarchar(512) NOT NULL,
	[Message] nvarchar(1024) NOT NULL,
	[Exception] nvarchar(max) NULL
)

IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Rulebase')
BEGIN
	EXEC('CREATE SCHEMA Rulebase')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- AEC_ApprenticeshipPriceEpisode
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='AEC_ApprenticeshipPriceEpisode' AND [schema_id] = SCHEMA_ID('Rulebase'))
BEGIN
	DROP TABLE Rulebase.AEC_ApprenticeshipPriceEpisode
END
GO

CREATE TABLE [Rulebase].[AEC_ApprenticeshipPriceEpisode]
(
	[LearnRefNumber] varchar(12),
	[AimSeqNumber] int,
	[EpisodeStartDate] date,
	[PriceEpisodeIdentifier] varchar(25),
	[PriceEpisodeActualEndDate] date,
	[PriceEpisodeActualInstalments] int,
	[PriceEpisodeBalanceValue] decimal(10,5),
	[PriceEpisodeCappedRemainingTNPAmount] decimal(10,5),
	[PriceEpisodeCompleted] bit,
	[PriceEpisodeCompletionElement] decimal(10,5),
	[PriceEpisodeExpectedTotalMonthlyValue] decimal(10,5),
	[PriceEpisodeInstalmentValue] decimal(10,5),
	[PriceEpisodePlannedEndDate] date,
	[PriceEpisodePlannedInstalments] int,
	[PriceEpisodePreviousEarnings] decimal(10,5),
	[PriceEpisodeRemainingAmountWithinUpperLimit] decimal(10,5),
	[PriceEpisodeRemainingTNPAmount] decimal(10,5),
	[PriceEpisodeTotalEarnings] decimal(10,5),
	[PriceEpisodeTotalTNPPrice] decimal(10,5),
	[PriceEpisodeUpperBandLimit] decimal(10,5),
	[PriceEpisodeUpperLimitAdjustment] decimal(10,5),
	[TNP1] decimal(10,5),
	[TNP2] decimal(10,5),
	[TNP3] decimal(10,5),
	[TNP4] decimal(10,5)
	PRIMARY KEY CLUSTERED
	(
		[LearnRefNumber] asc,
		[EpisodeStartDate] asc,
		[PriceEpisodeIdentifier] asc
	)
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- AEC_ApprenticeshipPriceEpisode_Period
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='AEC_ApprenticeshipPriceEpisode_Period' AND [schema_id] = SCHEMA_ID('Rulebase'))
BEGIN
	DROP TABLE Rulebase.AEC_ApprenticeshipPriceEpisode_Period
END
GO

CREATE TABLE [Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]
(
	[LearnRefNumber] varchar(12),
	[AimSeqNumber] int,
	[EpisodeStartDate] date,
	[PriceEpisodeIdentifier] varchar(25),
	[Period] int,
	[PriceEpisodeBalancePayment] decimal(10,5),
	[PriceEpisodeCompletionPayment] decimal(10,5),
	[PriceEpisodeInstalmentsThisPeriod] int,
	[PriceEpisodeOnProgPayment] decimal(10,5),
	PRIMARY KEY CLUSTERED
	(
		[LearnRefNumber] asc,
		[EpisodeStartDate] asc,
		[PriceEpisodeIdentifier] asc,
		[Period] asc
	)
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- AEC_ApprenticeshipPriceEpisode_PeriodisedValues
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='AEC_ApprenticeshipPriceEpisode_PeriodisedValues' AND [schema_id] = SCHEMA_ID('Rulebase'))
BEGIN
	DROP TABLE Rulebase.AEC_ApprenticeshipPriceEpisode_PeriodisedValues
END
GO

CREATE TABLE [Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
(
	[LearnRefNumber] varchar(12),
	[AimSeqNumber] int,
	[EpisodeStartDate] date,
	[PriceEpisodeIdentifier] varchar(25),
	[AttributeName] [varchar](100) not null,
	[Period_1] [decimal](15,5),
	[Period_2] [decimal](15,5),
	[Period_3] [decimal](15,5),
	[Period_4] [decimal](15,5),
	[Period_5] [decimal](15,5),
	[Period_6] [decimal](15,5),
	[Period_7] [decimal](15,5),
	[Period_8] [decimal](15,5),
	[Period_9] [decimal](15,5),
	[Period_10] [decimal](15,5),
	[Period_11] [decimal](15,5),
	[Period_12] [decimal](15,5),
	PRIMARY KEY CLUSTERED
	(
		[LearnRefNumber] asc,
		[EpisodeStartDate] asc,
		[PriceEpisodeIdentifier] asc,
		[AttributeName] asc
	)
)
GO
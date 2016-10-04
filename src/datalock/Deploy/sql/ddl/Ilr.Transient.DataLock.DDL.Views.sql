IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='DataLock')
BEGIN
	EXEC('CREATE SCHEMA DataLock')
END

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

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_Providers
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_Providers' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
	DROP VIEW DataLock.vw_Providers
END
GO

CREATE VIEW DataLock.vw_Providers
AS
SELECT
	p.UKPRN
FROM Input.LearningProvider p
GO
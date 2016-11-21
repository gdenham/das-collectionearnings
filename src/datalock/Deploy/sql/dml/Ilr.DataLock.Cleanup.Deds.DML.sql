DELETE FROM ${ILR_Deds.FQ}.[DataLock].[ValidationError]
    WHERE [Ukprn] IN (SELECT DISTINCT lp.[Ukprn] FROM [Valid].[LearningProvider] lp)
		AND [CollectionPeriodName] IS NULL
		AND [CollectionPeriodMonth] IS NULL
		AND [CollectionPeriodYear] IS NULL
GO

DELETE FROM ${ILR_Deds.FQ}.[DataLock].[DasLearnerCommitment]
    WHERE [Ukprn] IN (SELECT DISTINCT lp.[Ukprn] FROM [Valid].[LearningProvider] lp)
		AND [CollectionPeriodName] IS NULL
		AND [CollectionPeriodMonth] IS NULL
		AND [CollectionPeriodYear] IS NULL
GO

DELETE FROM ${ILR_Deds.FQ}.[DataLock].[ValidationError]
    WHERE [Ukprn] IN (SELECT DISTINCT lp.[Ukprn] FROM [Valid].[LearningProvider] lp)
GO

DELETE FROM ${ILR_Deds.FQ}.[DataLock].[DasLearnerCommitment]
    WHERE [Ukprn] IN (SELECT DISTINCT lp.[Ukprn] FROM [Valid].[LearningProvider] lp)
GO

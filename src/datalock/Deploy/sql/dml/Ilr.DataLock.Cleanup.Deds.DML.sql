DELETE FROM ${ILR_Deds.FQ}.[DataLock].[ValidationError]
    WHERE [Ukprn] IN (SELECT DISTINCT ve.[Ukprn] FROM [DataLock].[ValidationError] ve)
GO

DELETE FROM ${ILR_Deds.FQ}.[DataLock].[DasLearnerCommitment]
    WHERE [Ukprn] IN (SELECT DISTINCT dlc.[Ukprn] FROM [DataLock].[DasLearnerCommitment] dlc)
GO

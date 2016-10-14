DELETE FROM ${DAS_PeriodEnd.FQ}.[DataLock].[ValidationError]
    WHERE [Ukprn] IN (SELECT DISTINCT ve.[Ukprn] FROM [DataLock].[ValidationError] ve)
        AND [CollectionPeriodName] IN (SELECT [Collection_Period] FROM ${ILR_Summarisation.FQ}.dbo.Collection_Period_Mapping WHERE [Collection_Open] = 1)
        AND [CollectionPeriodMonth] IN (SELECT [Period] FROM ${ILR_Summarisation.FQ}.dbo.Collection_Period_Mapping WHERE [Collection_Open] = 1)
        AND [CollectionPeriodYear] IN (SELECT [Calendar_Year] FROM ${ILR_Summarisation.FQ}.dbo.Collection_Period_Mapping WHERE [Collection_Open] = 1)
GO

DELETE FROM ${DAS_PeriodEnd.FQ}.[DataLock].[DasLearnerCommitment]
    WHERE [Ukprn] IN (SELECT DISTINCT dlc.[Ukprn] FROM [DataLock].[DasLearnerCommitment] dlc)
        AND [CollectionPeriodName] IN (SELECT [Collection_Period] FROM ${ILR_Summarisation.FQ}.dbo.Collection_Period_Mapping WHERE [Collection_Open] = 1)
        AND [CollectionPeriodMonth] IN (SELECT [Period] FROM ${ILR_Summarisation.FQ}.dbo.Collection_Period_Mapping WHERE [Collection_Open] = 1)
        AND [CollectionPeriodYear] IN (SELECT [Calendar_Year] FROM ${ILR_Summarisation.FQ}.dbo.Collection_Period_Mapping WHERE [Collection_Open] = 1)
GO

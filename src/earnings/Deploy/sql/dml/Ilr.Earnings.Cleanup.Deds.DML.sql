DELETE FROM ${ILR_Deds.FQ}.Rulebase.AE_LearningDelivery
    WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO

DELETE FROM ${ILR_Deds.FQ}.Rulebase.AE_LearningDelivery_PeriodisedValues
    WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO

DELETE FROM ${ILR_Deds.FQ}.Rulebase.AE_LearningDelivery_Period
    WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO

TRUNCATE TABLE [Reference].[DasCommitments]
GO

INSERT INTO [Reference].[DasCommitments]
    SELECT
        [CommitmentId],
        MAX([VersionId]) [VersionId],
        [Uln],
        [Ukprn],
        [AccountId],
        [StartDate],
        [EndDate],
        [AgreedCost],
        [StandardCode],
        [ProgrammeType],
        [FrameworkCode],
        [PathwayCode],
        [PaymentStatus],
        [PaymentStatusDescription],
        [Payable],
        [Priority],
        [EffectiveFromDate],
        [EffectiveToDate]
    FROM ${DAS_Commitments.FQ}.[dbo].[DasCommitments]
    WHERE [Ukprn] IN (SELECT DISTINCT [Ukprn] FROM [Input].[LearningProvider])
    GROUP BY [CommitmentId],
        [Uln],
        [Ukprn],
        [AccountId],
        [StartDate],
        [EndDate],
        [AgreedCost],
        [StandardCode],
        [ProgrammeType],
        [FrameworkCode],
        [PathwayCode],
        [PaymentStatus],
        [PaymentStatusDescription],
        [Payable],
        [Priority],
        [EffectiveFromDate],
        [EffectiveToDate]
GO

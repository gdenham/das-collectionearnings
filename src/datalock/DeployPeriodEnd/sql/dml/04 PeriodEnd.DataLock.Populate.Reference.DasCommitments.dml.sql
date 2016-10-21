TRUNCATE TABLE [Reference].[DasCommitments]
GO

INSERT INTO [Reference].[DasCommitments]
    SELECT
        [CommitmentId],
        [Uln],
        [Ukprn],
        [AccountId],
        [StartDate],
        [EndDate],
        [AgreedCost],
        [StandardCode],
        [ProgrammeType],
        [FrameworkCode],
        [PathwayCode]
	FROM ${DAS_Commitments.FQ}.[dbo].[DasCommitments]
	WHERE [Ukprn] IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
GO

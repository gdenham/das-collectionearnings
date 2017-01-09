TRUNCATE TABLE [Reference].[DasCommitments]
GO

INSERT INTO [Reference].[DasCommitments]
    SELECT
        [CommitmentId],
        [VersionId],
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
GO

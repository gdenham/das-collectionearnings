DELETE FROM [Reference].[DasCommitments]
GO

INSERT INTO [Reference].[DasCommitments]
SELECT
	c.[CommitmentId],
	c.[Uln],
	c.[Ukprn],
	c.[AccountId],
	c.[StartDate],
	c.[EndDate],
	c.[AgreedCost],
	c.[StandardCode],
	c.[ProgrammeType],
	c.[FrameworkCode],
	c.[PathwayCode]
FROM ${DAS_Commitments.FQ}.dbo.DasCommitments c
    INNER JOIN [Input].[LearningProvider] p
        ON c.[Ukprn] = p.[Ukprn]
GO
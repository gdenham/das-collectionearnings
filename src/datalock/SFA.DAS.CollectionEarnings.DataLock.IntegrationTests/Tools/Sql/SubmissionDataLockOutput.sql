-- ValidationError
INSERT INTO ${ILR_Deds.FQ}.[DataLock].[ValidationError] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[RuleId]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn001', 
	1, 
	'DLOCK_02'
)
GO

INSERT INTO ${ILR_Deds.FQ}.[DataLock].[ValidationError] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[RuleId]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn002', 
	1, 
	'DLOCK_07'
)
GO

INSERT INTO ${ILR_Deds.FQ}.[DataLock].[ValidationError] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[RuleId]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn003', 
	1, 
	'DLOCK_03'
)
GO

-- DasLearnerCommitment
INSERT INTO ${ILR_Deds.FQ}.[DataLock].[DasLearnerCommitment] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[CommitmentId]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn099', 
	1, 
	12345
)
GO

INSERT INTO ${ILR_Deds.FQ}.[DataLock].[DasLearnerCommitment] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[CommitmentId]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn098', 
	1, 
	54321
)
GO

INSERT INTO ${ILR_Deds.FQ}.[DataLock].[DasLearnerCommitment] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[CommitmentId]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn099', 
	1, 
	10101
)
GO
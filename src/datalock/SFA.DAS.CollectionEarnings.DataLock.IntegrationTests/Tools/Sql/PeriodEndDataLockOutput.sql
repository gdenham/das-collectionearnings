﻿-- ValidationError
INSERT INTO ${DAS_PeriodEnd.FQ}.[DataLock].[ValidationError] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[RuleId],
    [CollectionPeriodName],
    [CollectionPeriodMonth],
    [CollectionPeriodYear]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn001', 
	1, 
	'DLOCK_02',
	(SELECT TOP 1 '1617-' + [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarMonth] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarYear] FROM Reference.CollectionPeriods WHERE [Open] = 1)
)
GO

INSERT INTO ${DAS_PeriodEnd.FQ}.[DataLock].[ValidationError] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[RuleId],
    [CollectionPeriodName],
    [CollectionPeriodMonth],
    [CollectionPeriodYear]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn002', 
	1, 
	'DLOCK_07',
	(SELECT TOP 1 '1617-' + [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarMonth] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarYear] FROM Reference.CollectionPeriods WHERE [Open] = 1)
)
GO

INSERT INTO ${DAS_PeriodEnd.FQ}.[DataLock].[ValidationError] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[RuleId],
    [CollectionPeriodName],
    [CollectionPeriodMonth],
    [CollectionPeriodYear]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn003', 
	1, 
	'DLOCK_03',
	(SELECT TOP 1 '1617-' + [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarMonth] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarYear] FROM Reference.CollectionPeriods WHERE [Open] = 1)
)
GO

-- DasLearnerCommitment
INSERT INTO ${DAS_PeriodEnd.FQ}.[DataLock].[DasLearnerCommitment] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[CommitmentId],
    [CollectionPeriodName],
    [CollectionPeriodMonth],
    [CollectionPeriodYear]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn099', 
	1, 
	12345,
	(SELECT TOP 1 '1617-' + [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarMonth] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarYear] FROM Reference.CollectionPeriods WHERE [Open] = 1)
)
GO

INSERT INTO ${DAS_PeriodEnd.FQ}.[DataLock].[DasLearnerCommitment] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[CommitmentId],
    [CollectionPeriodName],
    [CollectionPeriodMonth],
    [CollectionPeriodYear]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn098', 
	1, 
	54321,
	(SELECT TOP 1 '1617-' + [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarMonth] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarYear] FROM Reference.CollectionPeriods WHERE [Open] = 1)
)
GO

INSERT INTO ${DAS_PeriodEnd.FQ}.[DataLock].[DasLearnerCommitment] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[CommitmentId],
    [CollectionPeriodName],
    [CollectionPeriodMonth],
    [CollectionPeriodYear]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn099', 
	1, 
	10101,
	(SELECT TOP 1 '1617-' + [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarMonth] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarYear] FROM Reference.CollectionPeriods WHERE [Open] = 1)
)
GO
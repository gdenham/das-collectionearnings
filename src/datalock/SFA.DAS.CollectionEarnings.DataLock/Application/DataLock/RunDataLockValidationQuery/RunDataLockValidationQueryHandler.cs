using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery
{
    public class RunDataLockValidationQueryHandler : IRequestHandler<RunDataLockValidationQueryRequest, RunDataLockValidationQueryResponse>
    {
        private readonly MatchHandler _initialHandler;

        public RunDataLockValidationQueryHandler()
        {
            var ukprnMatcher = new UkprnMatchHandler();
            var ulnMatcher = new UlnMatchHandler();
            var standardMatcher = new StandardMatchHandler();
            var frameworkMatcher = new FrameworkMatchHandler();
            var programmeMatcher = new ProgrammeMatchHandler();
            var pathwaymatcher = new PathwayMatchHandler();
            var priceMatcher = new PriceMatchHandler();
            var startDateMatcher = new StartDateMatcher();
            var isPayableMatcher = new IsPayableMatchHandler();
            var multipleMatcher = new MultipleMatchHandler();

            ukprnMatcher.SetNextMatchHandler(ulnMatcher);
            ulnMatcher.SetNextMatchHandler(standardMatcher);
            standardMatcher.SetNextMatchHandler(frameworkMatcher);
            frameworkMatcher.SetNextMatchHandler(programmeMatcher);
            programmeMatcher.SetNextMatchHandler(pathwaymatcher);
            pathwaymatcher.SetNextMatchHandler(priceMatcher);
            priceMatcher.SetNextMatchHandler(startDateMatcher);
            startDateMatcher.SetNextMatchHandler(isPayableMatcher);
            isPayableMatcher.SetNextMatchHandler(multipleMatcher);

            _initialHandler = ukprnMatcher;
        }

        public RunDataLockValidationQueryResponse Handle(RunDataLockValidationQueryRequest message)
        {
            try
            {
                var validationErrors = new ConcurrentBag<Infrastructure.Data.Entities.ValidationErrorEntity>();
                var learnerCommitments = new ConcurrentBag<LearnerCommitment>();

                var learners = message.Learners.ToList();
                var partitioner = Partitioner.Create(0, learners.Count);

                Parallel.ForEach(partitioner, range =>
                {
                    for (var x = range.Item1; x < range.Item2; x++)
                    {
                        var learner = learners[x];

                        // Execute the matching chain
                        var matchResult = _initialHandler.Match(message.Commitments.ToList(), learner);

                        if (!string.IsNullOrEmpty(matchResult.ErrorCode))
                        {
                            validationErrors.Add(new Infrastructure.Data.Entities.ValidationErrorEntity
                            {
                                Ukprn = learner.Ukprn,
                                LearnRefNumber = learner.LearnerReferenceNumber,
                                AimSeqNumber = learner.AimSequenceNumber,
                                RuleId = matchResult.ErrorCode,
                                PriceEpisodeIdentifier=learner.PriceEpisodeIdentifier,
                                EpisodeStartDate=learner.LearningStartDate
                            });
                        }

                        if (matchResult.Commitment != null)
                        {
                            learnerCommitments.Add(new LearnerCommitment
                            {
                                Ukprn = learner.Ukprn,
                                LearnerReferenceNumber = learner.LearnerReferenceNumber,
                                AimSequenceNumber = learner.AimSequenceNumber ?? -1,
                                CommitmentId = matchResult.Commitment.CommitmentId,
                                PriceEpisodeIdentifier = learner.PriceEpisodeIdentifier
                            });
                        }
                    }
                });

                return new RunDataLockValidationQueryResponse
                {
                    IsValid = true,
                    ValidationErrors = validationErrors.ToArray(),
                    LearnerCommitments = learnerCommitments.ToArray()
                };
            }
            catch (Exception ex)
            {
                return new RunDataLockValidationQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}
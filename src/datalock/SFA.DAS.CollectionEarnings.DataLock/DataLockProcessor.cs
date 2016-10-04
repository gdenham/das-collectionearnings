using System.Linq;
using MediatR;
using NLog;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetAllCommitmentsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner.AddLearnerCommitmentsCommand;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner.GetAllLearnersQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock
{
    public class DataLockProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public DataLockProcessor(ILogger logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected DataLockProcessor()
        {
        }

        public virtual void Process()
        {
            _logger.Info("Started Data Lock Processor.");

            var commitments = ReturnCommitmentsOrThrow();
            var learners = ReturnValidGetAllLearnersQueryResponseOrThrow();

            if (learners.HasAnyItems())
            {
                _logger.Info("Started Data Lock Validation.");

                var dataLockValidationResult =
                    _mediator.Send(new RunDataLockValidationQueryRequest
                    {
                        Commitments = commitments,
                        Learners = learners.Items
                    });

                _logger.Info("Finished Data Lock Validation.");

                if (!dataLockValidationResult.IsValid)
                {
                    throw new DataLockProcessorException(DataLockProcessorException.ErrorPerformingDataLockMessage, dataLockValidationResult.Exception);
                }

                WriteDataLockValidationErrorsOrThrow(dataLockValidationResult.ValidationErrors);
                WriteDataLockMatchingLearnersAndCommitments(dataLockValidationResult.LearnerCommitments);
            }
            else
            {
                _logger.Info("No learners found.");
            }

            _logger.Info("Finished Data Lock Processor.");
        }

        private CommitmentEntity[] ReturnCommitmentsOrThrow()
        {
            _logger.Info("Reading commitments.");

            var commitments = _mediator.Send(new GetAllCommitmentsQueryRequest());

            if (!commitments.IsValid)
            {
                throw new DataLockProcessorException(DataLockProcessorException.ErrorReadingCommitmentsMessage, commitments.Exception);
            }

            return commitments.Items;
        }

        private GetAllLearnersQueryResponse ReturnValidGetAllLearnersQueryResponseOrThrow()
        {
            _logger.Info("Reading learners.");

            var learnersQueryResponse = _mediator.Send(new GetAllLearnersQueryRequest());

            if (!learnersQueryResponse.IsValid)
            {
                throw new DataLockProcessorException(DataLockProcessorException.ErrorReadingLearnersMessage, learnersQueryResponse.Exception);
            }

            return learnersQueryResponse;
        }

        private void WriteDataLockValidationErrorsOrThrow(ValidationErrorEntity[] errors)
        {
            if (errors.Any())
            {
                _logger.Info("Started writing Data Lock Validation Errors.");

                var writeValidationErrorsResult =
                    _mediator.Send(new AddValidationErrorsCommandRequest
                    {
                        ValidationErrors = errors
                    });

                if (!writeValidationErrorsResult.IsValid)
                {
                    throw new DataLockProcessorException(DataLockProcessorException.ErrorWritingDataLockValidationErrorsMessage, writeValidationErrorsResult.Exception);
                }

                _logger.Info("Finished writing Data Lock Validation Errors.");
            }
        }

        private void WriteDataLockMatchingLearnersAndCommitments(LearnerCommitment[] matchingLearnersAndCommitments)
        {
            if (matchingLearnersAndCommitments.Any())
            {
                _logger.Info("Started writing matching Learners and Commitments.");

                var writeMatchingLearnersAndCommitments =
                    _mediator.Send(new AddLearnerCommitmentsCommandRequest
                    {
                        LearnerCommitments = matchingLearnersAndCommitments
                    });

                if (!writeMatchingLearnersAndCommitments.IsValid)
                {
                    throw new DataLockProcessorException(DataLockProcessorException.ErrorWritingMatchingLearnersAndCommitmentsMessage, writeMatchingLearnersAndCommitments.Exception);
                }

                _logger.Info("Finished writing matching Learners and Commitments.");
            }
        }
    }
}

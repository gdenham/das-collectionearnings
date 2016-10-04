using System.Linq;
using MediatR;
using NLog;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetAllCommitmentsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner.AddLearnerCommitmentsCommand;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner.GetAllLearnersQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand;

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

            _logger.Info("Reading commitments.");

            var commitments = _mediator.Send(new GetAllCommitmentsQueryRequest());

            if (!commitments.IsValid)
            {
                throw new DataLockProcessorException(DataLockProcessorException.ErrorReadingCommitmentsMessage, commitments.Exception);
            }

            _logger.Info("Reading learners.");

            var learners = _mediator.Send(new GetAllLearnersQueryRequest());

            if (!learners.IsValid)
            {
                throw new DataLockProcessorException(DataLockProcessorException.ErrorReadingLearnersMessage, learners.Exception);
            }

            if (learners.HasAnyItems())
            {
                _logger.Info("Started Data Lock Validation.");

                var dataLockValidationErrors =
                    _mediator.Send(new RunDataLockValidationQueryRequest
                    {
                        Commitments = commitments.Items,
                        Learners = learners.Items
                    });

                _logger.Info("Finished Data Lock Validation.");

                if (!dataLockValidationErrors.IsValid)
                {
                    throw new DataLockProcessorException(DataLockProcessorException.ErrorPerformingDataLockMessage, dataLockValidationErrors.Exception);
                }

                if (dataLockValidationErrors.ValidationErrors.Any())
                {
                    _logger.Info("Started writing Data Lock Validation Errors.");

                    var writeValidationErrorsResult =
                        _mediator.Send(new AddValidationErrorsCommandRequest
                        {
                            ValidationErrors = dataLockValidationErrors.ValidationErrors
                        });

                    if (!writeValidationErrorsResult.IsValid)
                    {
                        throw new DataLockProcessorException(DataLockProcessorException.ErrorWritingDataLockValidationErrorsMessage, writeValidationErrorsResult.Exception);
                    }

                    _logger.Info("Finished writing Data Lock Validation Errors.");
                }

                if (dataLockValidationErrors.LearnerCommitments.Any())
                {
                    _logger.Info("Started writing matching Learners and Commitments.");

                    var writeMatchingLearnersAndCommitments =
                        _mediator.Send(new AddLearnerCommitmentsCommandRequest
                        {
                            LearnerCommitments = dataLockValidationErrors.LearnerCommitments
                        });

                    if (!writeMatchingLearnersAndCommitments.IsValid)
                    {
                        throw new DataLockProcessorException(DataLockProcessorException.ErrorWritingMatchingLearnersAndCommitmentsMessage, writeMatchingLearnersAndCommitments.Exception);
                    }

                    _logger.Info("Finished writing matching Learners and Commitments.");
                }
            }
            else
            {
                _logger.Info("No DAS learners found.");
            }

            _logger.Info("Finished Data Lock Processor.");
        }
    }
}

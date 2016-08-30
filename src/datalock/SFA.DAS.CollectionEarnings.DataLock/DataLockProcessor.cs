using System.Linq;
using MediatR;
using NLog;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetAllCommitmentsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasLearner.GetAllDasLearnersQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.GetDataLockFailuresQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand;
using SFA.DAS.CollectionEarnings.DataLock.Exceptions;

namespace SFA.DAS.CollectionEarnings.DataLock
{
    internal class DataLockProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public DataLockProcessor(ILogger logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public void Process()
        {
            _logger.Info("Started Data Lock Processor.");

            _logger.Debug("Reading commitments.");

            var commitments = _mediator.Send(new GetAllCommitmentsQueryRequest());

            if (!commitments.IsValid)
            {
                throw new DataLockProcessorException("Error while reading commitments.", commitments.Exception);
            }

            _logger.Debug("Reading DAS learners.");

            var dasLearners = _mediator.Send(new GetAllDasLearnersQueryRequest());

            if (!dasLearners.IsValid)
            {
                throw new DataLockProcessorException("Error while reading DAS specific learners.", dasLearners.Exception);
            }

            _logger.Debug("Started Data Lock Validation.");

            var dataLockValidationErrors =
                _mediator.Send(new GetDataLockFailuresQueryRequest
                {
                    Commitments = commitments.Items,
                    DasLearners = dasLearners.Items
                });

            _logger.Debug("Finished Data Lock Validation.");

            if (!dataLockValidationErrors.IsValid)
            {
                throw new DataLockProcessorException("Error while performing data lock.", dataLockValidationErrors.Exception);
            }

            if (dataLockValidationErrors.Items.Any())
            {
                _logger.Debug("Started writing Data Lock Validation Errors");

                var writeValidationErrorsResult =
                    _mediator.Send(new AddValidationErrorsCommandRequest
                    {
                        ValidationErrors = dataLockValidationErrors.Items
                    });

                if (!writeValidationErrorsResult.IsValid)
                {
                    throw new DataLockProcessorException("Error while writing data lock validation errors.", writeValidationErrorsResult.Exception);
                }

                _logger.Debug("Finished writing Data Lock Validation Errors");
            }

            _logger.Info("Finished Data Lock Processor.");
        }
    }
}

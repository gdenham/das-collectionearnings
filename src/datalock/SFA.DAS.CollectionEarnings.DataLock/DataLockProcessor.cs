using System;
using MediatR;
using NLog;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetProviderCommitmentsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner.AddLearnerCommitmentsCommand;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisode;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisode.GetProviderPriceEpisodesQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.Provider.GetProvidersQuery;
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

            var providersQueryResponse = ReturnValidGetProvidersQueryResponseOrThrow();

            if (providersQueryResponse.HasAnyItems())
            {
                foreach (var provider in providersQueryResponse.Items)
                {
                    _logger.Info($"Performing Data Lock Validation for provider with ukprn {provider.Ukprn}.");

                    var commitments = ReturnProviderCommitmentsOrThrow(provider.Ukprn);
                    var priceEpisodes = ReturnValidGetProviderPriceEpisodesQueryResponseOrThrow(provider.Ukprn);

                    if (priceEpisodes.HasAnyItems())
                    {
                        var dataLockValidationResult = ReturnDataLockValidationResultOrThrow(commitments, priceEpisodes.Items);

                        WriteDataLockValidationErrorsOrThrow(dataLockValidationResult);
                        WriteDataLockMatchingLearnersAndCommitments(dataLockValidationResult);
                    }
                    else
                    {
                        _logger.Info("No price episodes found.");
                    }
                }
            }
            else
            {
                _logger.Info("No providers found to process.");
            }

            _logger.Info("Finished Data Lock Processor.");
        }

        private GetProvidersQueryResponse ReturnValidGetProvidersQueryResponseOrThrow()
        {
            var providersQueryResponse = _mediator.Send(new GetProvidersQueryRequest());

            if (!providersQueryResponse.IsValid)
            {
                throw new DataLockProcessorException(DataLockProcessorException.ErrorReadingProviders, providersQueryResponse.Exception);
            }

            return providersQueryResponse;
        }

        private Commitment[] ReturnProviderCommitmentsOrThrow(long ukprn)
        {
            _logger.Info($"Reading commitments for provider with ukprn {ukprn}.");

            var commitments =
                _mediator.Send(new GetProviderCommitmentsQueryRequest
                {
                    Ukprn = ukprn
                });

            if (!commitments.IsValid)
            {
                throw new DataLockProcessorException(DataLockProcessorException.ErrorReadingCommitmentsMessage, commitments.Exception);
            }

            return commitments.Items;
        }

        private GetProviderPriceEpisodesQueryResponse ReturnValidGetProviderPriceEpisodesQueryResponseOrThrow(long ukprn)
        {
            _logger.Info($"Reading price episodes for provider with ukprn {ukprn}.");

            var priceEpisodesQueryResponse = _mediator.Send(new GetProviderPriceEpisodesQueryRequest
            {
                Ukprn = ukprn
            });

            if (!priceEpisodesQueryResponse.IsValid)
            {
                throw new DataLockProcessorException(DataLockProcessorException.ErrorReadingPriceEpisodesMessage, priceEpisodesQueryResponse.Exception);
            }

            return priceEpisodesQueryResponse;
        }

        private RunDataLockValidationQueryResponse ReturnDataLockValidationResultOrThrow(Commitment[] commitments, PriceEpisode[] priceEpisodes)
        {
            _logger.Info("Started Data Lock Validation.");

            var dataLockValidationResult =
                _mediator.Send(new RunDataLockValidationQueryRequest
                {
                    Commitments = commitments,
                    PriceEpisodes = priceEpisodes
                });

            _logger.Info("Finished Data Lock Validation.");

            if (!dataLockValidationResult.IsValid)
            {
                throw new DataLockProcessorException(DataLockProcessorException.ErrorPerformingDataLockMessage, dataLockValidationResult.Exception);
            }

            return dataLockValidationResult;
        }

        private void WriteDataLockValidationErrorsOrThrow(RunDataLockValidationQueryResponse dataLockValidationResponse)
        {
            if (dataLockValidationResponse.HasAnyValidationErrors())
            {
                _logger.Info("Started writing Data Lock Validation Errors.");

                try
                {
                    _mediator.Send(new AddValidationErrorsCommandRequest
                    {
                        ValidationErrors = dataLockValidationResponse.ValidationErrors
                    });
                }
                catch (Exception ex)
                {
                    throw new DataLockProcessorException(DataLockProcessorException.ErrorWritingDataLockValidationErrorsMessage, ex);
                }

                _logger.Info("Finished writing Data Lock Validation Errors.");
            }
        }

        private void WriteDataLockMatchingLearnersAndCommitments(RunDataLockValidationQueryResponse dataLockValidationResponse)
        {
            if (dataLockValidationResponse.HasAnyMatchingLearnersAndCommitments())
            {
                _logger.Info("Started writing matching Learners and Commitments.");

                try
                {
                    _mediator.Send(new AddLearnerCommitmentsCommandRequest
                    {
                        LearnerCommitments = dataLockValidationResponse.LearnerCommitments
                    });
                }
                catch (Exception ex)
                {
                    throw new DataLockProcessorException(DataLockProcessorException.ErrorWritingMatchingLearnersAndCommitmentsMessage, ex);
                }

                _logger.Info("Finished writing matching Learners and Commitments.");
            }
        }
    }
}

using System.Linq;
using MediatR;
using NLog;
using SFA.DAS.CollectionEarnings.Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery;
using SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery;
using SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDelivery.AddProcessedLearningDeliveriesCommand;
using SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDeliveryPeriodisedValues.AddProcessedLearningDeliveryPeriodisedValuesCommand;
using SFA.DAS.CollectionEarnings.Calculator.Exceptions;

namespace SFA.DAS.CollectionEarnings.Calculator
{
    public class ApprenticeshipEarningsProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public ApprenticeshipEarningsProcessor(ILogger logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected ApprenticeshipEarningsProcessor()
        {
        }

        public virtual void Process()
        {
            _logger.Info("Started Apprenticeship Earnings Processor.");

            _logger.Debug("Reading learning deliveries to process.");

            var learningDeliveries = _mediator.Send(new GetAllLearningDeliveriesToProcessQueryRequest());

            if (!learningDeliveries.IsValid)
            {
                throw new EarningsCalculatorProcessorException(EarningsCalculatorExceptionMessages.ErrorReadingLearningDeliveriesToProcess, learningDeliveries.Exception);
            }

            if (learningDeliveries.Items != null && learningDeliveries.Items.Any())
            {
                _logger.Debug("Started calculating the earnings for the found learning deliveries.");

                var earnings = _mediator.Send(new GetLearningDeliveriesEarningsQueryRequest
                {
                    LearningDeliveries = learningDeliveries.Items
                });

                _logger.Debug("Finished calculating the earnings for the found learning deliveries.");

                if (!earnings.IsValid)
                {
                    throw new EarningsCalculatorProcessorException(EarningsCalculatorExceptionMessages.ErrorCalculatingEarningsForTheLearningDeliveries, earnings.Exception);
                }

                _logger.Debug("Started writing processed learning deliveries.");

                var writeProcessedLearningDeliveriesResult =
                    _mediator.Send(new AddProcessedLearningDeliveriesCommandRequest
                    {
                        LearningDeliveries = earnings.ProcessedLearningDeliveries
                    });

                _logger.Debug("Finished writing processed learning deliveries.");

                if (!writeProcessedLearningDeliveriesResult.IsValid)
                {
                    throw new EarningsCalculatorProcessorException(EarningsCalculatorExceptionMessages.ErrorWritingProcessedLearningDeliveries, writeProcessedLearningDeliveriesResult.Exception);
                }

                _logger.Debug("Started writing processed learning deliveries periodised values.");

                var writeProcessedLearningDeliveryPeriodisedValuesResult =
                    _mediator.Send(new AddProcessedLearningDeliveryPeriodisedValuesCommandRequest
                    {
                        PeriodisedValues = earnings.ProcessedLearningDeliveryPeriodisedValues
                    });

                _logger.Debug("Finished writing processed learning deliveries periodised values.");

                if (!writeProcessedLearningDeliveryPeriodisedValuesResult.IsValid)
                {
                    throw new EarningsCalculatorProcessorException(EarningsCalculatorExceptionMessages.ErrorWritingProcessedLearningDeliveryPeriodisedValues, writeProcessedLearningDeliveryPeriodisedValuesResult.Exception);
                }
            }
            else
            {
                _logger.Info("Not found any learning deliveries to process.");
            }

            _logger.Info("Finished Apprenticeship Earnings Processor.");
        }
    }
}
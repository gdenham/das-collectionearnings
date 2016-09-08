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
                throw new EarningsCalculatorProcessorException("Error while reading learning deliveries to process.", learningDeliveries.Exception);
            }

            if (learningDeliveries.Items != null && learningDeliveries.Items.Any())
            {
                var earnings = _mediator.Send(new GetLearningDeliveriesEarningsQueryRequest
                {
                    LearningDeliveries = learningDeliveries.Items
                });

                if (!earnings.IsValid)
                {
                    throw new EarningsCalculatorProcessorException("Error while processing the learning deliveries to calculate the earnings.", earnings.Exception);
                }

                var writeProcessedLearningDeliveriesResult =
                    _mediator.Send(new AddProcessedLearningDeliveriesCommandRequest
                    {
                        LearningDeliveries = earnings.ProcessedLearningDeliveries
                    });

                if (!writeProcessedLearningDeliveriesResult.IsValid)
                {
                    throw new EarningsCalculatorProcessorException("Error while writing processed learning deliveries.", writeProcessedLearningDeliveriesResult.Exception);
                }

                var writeProcessedLearningDeliveryPeriodisedValuesResult =
                    _mediator.Send(new AddProcessedLearningDeliveryPeriodisedValuesCommandRequest
                    {
                        PeriodisedValues = earnings.ProcessedLearningDeliveryPeriodisedValues
                    });

                if (!writeProcessedLearningDeliveryPeriodisedValuesResult.IsValid)
                {
                    throw new EarningsCalculatorProcessorException("Error while writing processed learning deliveries periodised values.", writeProcessedLearningDeliveryPeriodisedValuesResult.Exception);
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
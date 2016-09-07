using System.Linq;
using MediatR;
using NLog;
using SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery;
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
                
            }
            else
            {
                _logger.Info("Not found any learning deliveries to process.");
            }

            _logger.Info("Finished Apprenticeship Earnings Processor.");
        }
    }
}
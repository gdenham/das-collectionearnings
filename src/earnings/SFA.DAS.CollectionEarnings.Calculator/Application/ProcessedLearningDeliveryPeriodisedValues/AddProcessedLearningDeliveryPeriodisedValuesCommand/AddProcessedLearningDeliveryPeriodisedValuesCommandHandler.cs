using MediatR;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDeliveryPeriodisedValues.AddProcessedLearningDeliveryPeriodisedValuesCommand
{
    public class AddProcessedLearningDeliveryPeriodisedValuesCommandHandler : IRequestHandler<AddProcessedLearningDeliveryPeriodisedValuesCommandRequest, Unit>
    {
        private readonly IProcessedLearningDeliveryPeriodisedValuesRepository _repository;

        public AddProcessedLearningDeliveryPeriodisedValuesCommandHandler(IProcessedLearningDeliveryPeriodisedValuesRepository repository)
        {
            _repository = repository;
        }

        public Unit Handle(AddProcessedLearningDeliveryPeriodisedValuesCommandRequest message)
        {
            _repository.AddProcessedLearningDeliveryPeriodisedValues(message.PeriodisedValues);

            return Unit.Value;
        }
    }
}
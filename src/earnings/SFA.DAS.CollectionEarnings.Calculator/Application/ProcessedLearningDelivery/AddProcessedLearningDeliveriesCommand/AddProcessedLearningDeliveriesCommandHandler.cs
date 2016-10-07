using MediatR;
using SFA.DAS.CollectionEarnings.Calculator.Data;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDelivery.AddProcessedLearningDeliveriesCommand
{
    public class AddProcessedLearningDeliveriesCommandHandler : IRequestHandler<AddProcessedLearningDeliveriesCommandRequest, Unit>
    {
        private readonly IProcessedLearningDeliveryRepository _repository;

        public AddProcessedLearningDeliveriesCommandHandler(IProcessedLearningDeliveryRepository repository)
        {
            _repository = repository;
        }

        public Unit Handle(AddProcessedLearningDeliveriesCommandRequest message)
        {
            _repository.AddProcessedLearningDeliveries(message.LearningDeliveries);

            return Unit.Value;
        }
    }
}
using System;
using MediatR;
using SFA.DAS.CollectionEarnings.Calculator.Data.Repositories;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDelivery.AddProcessedLearningDeliveriesCommand
{
    public class AddProcessedLearningDeliveriesCommandHandler : IRequestHandler<AddProcessedLearningDeliveriesCommandRequest, AddProcessedLearningDeliveriesCommandResponse>
    {
        private readonly IProcessedLearningDeliveryRepository _repository;

        public AddProcessedLearningDeliveriesCommandHandler(IProcessedLearningDeliveryRepository repository)
        {
            _repository = repository;
        }

        public AddProcessedLearningDeliveriesCommandResponse Handle(AddProcessedLearningDeliveriesCommandRequest message)
        {
            try
            {
                _repository.AddProcessedLearningDeliveries(message.LearningDeliveries);

                return new AddProcessedLearningDeliveriesCommandResponse
                {
                    IsValid = true
                };
            }
            catch (Exception ex)
            {
                return new AddProcessedLearningDeliveriesCommandResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}
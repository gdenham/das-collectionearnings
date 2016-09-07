using System;
using MediatR;
using SFA.DAS.CollectionEarnings.Calculator.Data.Repositories;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDeliveryPeriodisedValues.AddProcessedLearningDeliveryPeriodisedValuesCommand
{
    public class AddProcessedLearningDeliveryPeriodisedValuesCommandHandler : IRequestHandler<AddProcessedLearningDeliveryPeriodisedValuesCommandRequest, AddProcessedLearningDeliveryPeriodisedValuesCommandResponse>
    {
        private readonly IProcessedLearningDeliveryPeriodisedValuesRepository _repository;

        public AddProcessedLearningDeliveryPeriodisedValuesCommandHandler(IProcessedLearningDeliveryPeriodisedValuesRepository repository)
        {
            _repository = repository;
        }

        public AddProcessedLearningDeliveryPeriodisedValuesCommandResponse Handle(AddProcessedLearningDeliveryPeriodisedValuesCommandRequest message)
        {
            try
            {
                _repository.AddProcessedLearningDeliveryPeriodisedValues(message.PeriodisedValueses);

                return new AddProcessedLearningDeliveryPeriodisedValuesCommandResponse
                {
                    IsValid = true
                };
            }
            catch (Exception ex)
            {
                return new AddProcessedLearningDeliveryPeriodisedValuesCommandResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}
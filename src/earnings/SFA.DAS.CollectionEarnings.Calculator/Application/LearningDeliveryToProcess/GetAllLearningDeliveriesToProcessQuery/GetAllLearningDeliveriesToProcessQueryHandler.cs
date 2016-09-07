using System;
using MediatR;
using SFA.DAS.CollectionEarnings.Calculator.Data.Repositories;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery
{
    public class GetAllLearningDeliveriesToProcessQueryHandler : IRequestHandler<GetAllLearningDeliveriesToProcessQueryRequest, GetAllLearningDeliveriesToProcessQueryResponse>
    {
        private readonly ILearningDeliveryToProcessRepository _learningDeliveryToProcessRepository;

        public GetAllLearningDeliveriesToProcessQueryHandler(ILearningDeliveryToProcessRepository learningDeliveryToProcessRepository)
        {
            _learningDeliveryToProcessRepository = learningDeliveryToProcessRepository;
        }

        public GetAllLearningDeliveriesToProcessQueryResponse Handle(GetAllLearningDeliveriesToProcessQueryRequest message)
        {
            try
            {
                return new GetAllLearningDeliveriesToProcessQueryResponse
                {
                    IsValid = true,
                    Items = _learningDeliveryToProcessRepository.GetAllLearningDeliveriesToProcess()
                };
            }
            catch (Exception ex)
            {
                return new GetAllLearningDeliveriesToProcessQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}
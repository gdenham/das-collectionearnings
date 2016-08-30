using System;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Data.Repositories;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DasLearner.GetAllDasLearnersQuery
{
    public class GetAllDasLearnersQueryHandler : IRequestHandler<GetAllDasLearnersQueryRequest, GetAllDasLearnersQueryResponse>
    {
        private readonly IDasLearnerRepository _dasLearnerRepository;

        public GetAllDasLearnersQueryHandler(IDasLearnerRepository dasLearnerRepository)
        {
            _dasLearnerRepository = dasLearnerRepository;
        }

        public GetAllDasLearnersQueryResponse Handle(GetAllDasLearnersQueryRequest message)
        {
            try
            {
                return new GetAllDasLearnersQueryResponse
                {
                    IsValid = true,
                    Items = _dasLearnerRepository.GetAllDasLearners()
                };
            }
            catch (Exception ex)
            {
                return new GetAllDasLearnersQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}
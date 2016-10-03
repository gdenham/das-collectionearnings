using System;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.Learner.GetAllLearnersQuery
{
    public class GetAllLearnersQueryHandler : IRequestHandler<GetAllLearnersQueryRequest, GetAllLearnersQueryResponse>
    {
        private readonly ILearnerRepository _dasLearnerRepository;

        public GetAllLearnersQueryHandler(ILearnerRepository dasLearnerRepository)
        {
            _dasLearnerRepository = dasLearnerRepository;
        }

        public GetAllLearnersQueryResponse Handle(GetAllLearnersQueryRequest message)
        {
            try
            {
                return new GetAllLearnersQueryResponse
                {
                    IsValid = true,
                    Items = _dasLearnerRepository.GetAllLearners()
                };
            }
            catch (Exception ex)
            {
                return new GetAllLearnersQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}
using System;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Data.Repositories;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetAllCommitmentsQuery
{
    public class GetAllCommitmentsQueryHandler : IRequestHandler<GetAllCommitmentsQueryRequest, GetAllCommitmentsQueryResponse>
    {
        private readonly ICommitmentRepository _commitmentRepository;

        public GetAllCommitmentsQueryHandler(ICommitmentRepository commitmentRepository)
        {
            _commitmentRepository = commitmentRepository;
        }

        public GetAllCommitmentsQueryResponse Handle(GetAllCommitmentsQueryRequest message)
        {
            try
            {
                return new GetAllCommitmentsQueryResponse
                {
                    IsValid = true,
                    Items = _commitmentRepository.GetAllCommitments()
                };
            }
            catch (Exception ex)
            {
                return new GetAllCommitmentsQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}

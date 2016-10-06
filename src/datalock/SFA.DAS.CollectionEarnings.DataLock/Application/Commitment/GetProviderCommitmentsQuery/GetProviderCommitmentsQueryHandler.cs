using System;
using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetProviderCommitmentsQuery
{
    public class GetProviderCommitmentsQueryHandler : IRequestHandler<GetProviderCommitmentsQueryRequest, GetProviderCommitmentsQueryResponse>
    {
        private readonly ICommitmentRepository _commitmentRepository;

        public GetProviderCommitmentsQueryHandler(ICommitmentRepository commitmentRepository)
        {
            _commitmentRepository = commitmentRepository;
        }

        public GetProviderCommitmentsQueryResponse Handle(GetProviderCommitmentsQueryRequest message)
        {
            try
            {
                var commitmentEntities = _commitmentRepository.GetProviderCommitments(message.Ukprn);

                return new GetProviderCommitmentsQueryResponse
                {
                    IsValid = true,
                    Items = commitmentEntities?.Select(c =>
                        new Commitment
                        {
                            CommitmentId = c.CommitmentId,
                            Uln = c.Uln,
                            Ukprn = c.Ukprn,
                            AccountId = c.AccountId,
                            StartDate = c.StartDate,
                            EndDate = c.EndDate,
                            NegotiatedPrice = c.AgreedCost,
                            StandardCode = c.StandardCode,
                            FrameworkCode = c.FrameworkCode,
                            ProgrammeType = c.ProgrammeType,
                            PathwayCode = c.PathwayCode
                        }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetProviderCommitmentsQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}

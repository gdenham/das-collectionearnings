using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.Learner.GetProviderLearnersQuery
{
    public class GetProviderLearnersQueryRequest : IRequest<GetProviderLearnersQueryResponse>
    {
        public long Ukprn { get; set; }
    }
}
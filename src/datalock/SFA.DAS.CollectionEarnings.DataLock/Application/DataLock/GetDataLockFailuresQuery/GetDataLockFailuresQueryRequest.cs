using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.GetDataLockFailuresQuery
{
    public class GetDataLockFailuresQueryRequest : IRequest<GetDataLockFailuresQueryResponse>
    {
         public IEnumerable<Data.Entities.Commitment> Commitments { get; set; }
        public IEnumerable<Data.Entities.DasLearner> DasLearners { get; set; }  
    }
}
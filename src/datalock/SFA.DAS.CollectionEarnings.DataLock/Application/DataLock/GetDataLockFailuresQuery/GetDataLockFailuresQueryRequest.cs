using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.GetDataLockFailuresQuery
{
    public class GetDataLockFailuresQueryRequest : IRequest<GetDataLockFailuresQueryResponse>
    {
         public IEnumerable<Infrastructure.Data.Entities.CommitmentEntity> Commitments { get; set; }
        public IEnumerable<Infrastructure.Data.Entities.LearnerEntity> DasLearners { get; set; }  
    }
}
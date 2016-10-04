using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery
{
    public class RunDataLockValidationQueryRequest : IRequest<RunDataLockValidationQueryResponse>
    {
         public IEnumerable<Infrastructure.Data.Entities.CommitmentEntity> Commitments { get; set; }
        public IEnumerable<Infrastructure.Data.Entities.LearnerEntity> Learners { get; set; }  
    }
}
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Application;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery
{
    public class RunDataLockValidationQueryResponse : Response
    {
        public ValidationErrorEntity[] ValidationErrors { get; set; }
        public LearnerCommitment[] LearnerCommitments { get; set; }

        public bool HasAnyValidationErrors()
        {
            return ValidationErrors != null && ValidationErrors.Any();
        }

        public bool HasAnyMatchingLearnersAndCommitments()
        {
            return LearnerCommitments != null && LearnerCommitments.Any();
        }
    }
}

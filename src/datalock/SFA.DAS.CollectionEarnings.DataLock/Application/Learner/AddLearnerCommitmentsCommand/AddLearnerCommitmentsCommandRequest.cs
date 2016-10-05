using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.Learner.AddLearnerCommitmentsCommand
{
    public class AddLearnerCommitmentsCommandRequest : IRequest
    {
         public LearnerCommitment[] LearnerCommitments { get; set; }
    }
}
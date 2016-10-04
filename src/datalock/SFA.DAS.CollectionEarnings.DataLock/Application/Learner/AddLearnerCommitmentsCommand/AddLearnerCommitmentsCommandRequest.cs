using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.Learner.AddLearnerCommitmentsCommand
{
    public class AddLearnerCommitmentsCommandRequest : IRequest<AddLearnerCommitmentsCommandResponse>
    {
         public LearnerCommitment[] LearnerCommitments { get; set; }
    }
}
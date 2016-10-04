using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.Learner.AddLearnerCommitmentsCommand
{
    public class AddLearnerCommitmentsCommandHandler : IRequestHandler<AddLearnerCommitmentsCommandRequest, Unit>
    {
        private readonly ILearnerCommitmentRepository _learnerCommitmentRepository;

        public AddLearnerCommitmentsCommandHandler(ILearnerCommitmentRepository learnerCommitmentRepository)
        {
            _learnerCommitmentRepository = learnerCommitmentRepository;
        }

        public Unit Handle(AddLearnerCommitmentsCommandRequest message)
        {
            var learnerCommitmentEntities = message.LearnerCommitments
                .Select(
                    lc => new LearnerCommitmentEntity
                    {
                        Ukprn = lc.Ukprn,
                        LearnRefNumber = lc.LearnerReferenceNumber,
                        AimSeqNumber = lc.AimSequenceNumber,
                        CommitmentId = lc.CommitmentId
                    })
                .ToArray();

            _learnerCommitmentRepository.AddLearnerCommitments(learnerCommitmentEntities);

            return Unit.Value;
        }
    }
}
using System;
using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.Learner.AddLearnerCommitmentsCommand
{
    public class AddLearnerCommitmentsCommandHandler : IRequestHandler<AddLearnerCommitmentsCommandRequest, AddLearnerCommitmentsCommandResponse>
    {
        private readonly ILearnerCommitmentRepository _learnerCommitmentRepository;

        public AddLearnerCommitmentsCommandHandler(ILearnerCommitmentRepository learnerCommitmentRepository)
        {
            _learnerCommitmentRepository = learnerCommitmentRepository;
        }

        public AddLearnerCommitmentsCommandResponse Handle(AddLearnerCommitmentsCommandRequest message)
        {
            try
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

                return new AddLearnerCommitmentsCommandResponse
                {
                    IsValid = true
                };
            }
            catch (Exception ex)
            {
                return new AddLearnerCommitmentsCommandResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}
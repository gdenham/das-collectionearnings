using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDeliveryPeriod.AddProcessedLearningDeliveryPeriodCommand
{
    public class AddProcessedLearningDeliveryPeriodCommandHandler: IRequestHandler<AddProcessedLearningDeliveryPeriodCommandRequest, Unit>
    {
        private readonly IProcessedLearningDeliveryPeriodRepository _repository;

        public AddProcessedLearningDeliveryPeriodCommandHandler(IProcessedLearningDeliveryPeriodRepository repository)
        {
            _repository = repository;
        }

        public Unit Handle(AddProcessedLearningDeliveryPeriodCommandRequest message)
        {
            var periodEarnings = message.PeriodEarnings
                .Select(pe =>
                    new Infrastructure.Data.Entities.ProcessedLearningDeliveryPeriod
                    {
                        LearnRefNumber = pe.LearnerReferenceNumber,
                        AimSeqNumber = pe.AimSequenceNumber,
                        Period = pe.Period,
                        ProgrammeAimOnProgPayment = pe.OnProgrammeEarning,
                        ProgrammeAimCompletionPayment = pe.CompletionEarning,
                        ProgrammeAimBalPayment = pe.BalancingEarning
                    })
                .ToArray();

            _repository.AddProcessedLearningDeliveryPeriod(periodEarnings);

            return Unit.Value;
        }
    }
}
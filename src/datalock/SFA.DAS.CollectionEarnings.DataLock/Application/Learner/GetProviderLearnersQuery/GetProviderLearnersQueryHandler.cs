using System;
using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.Learner.GetProviderLearnersQuery
{
    public class GetProviderLearnersQueryHandler : IRequestHandler<GetProviderLearnersQueryRequest, GetProviderLearnersQueryResponse>
    {
        private readonly ILearnerRepository _learnerRepository;

        public GetProviderLearnersQueryHandler(ILearnerRepository learnerRepository)
        {
            _learnerRepository = learnerRepository;
        }

        public GetProviderLearnersQueryResponse Handle(GetProviderLearnersQueryRequest message)
        {
            try
            {
                var learnerEntities = _learnerRepository.GetProviderLearners(message.Ukprn);

                return new GetProviderLearnersQueryResponse
                {
                    IsValid = true,
                    Items = learnerEntities?.Select(l =>
                    new Learner
                    {
                        Ukprn = l.Ukprn,
                        LearnerReferenceNumber = l.LearnRefNumber,
                        Uln = l.Uln,
                        NiNumber = l.NiNumber,
                        AimSequenceNumber = l.AimSeqNumber,
                        StandardCode = l.StandardCode,
                        ProgrammeType = l.ProgrammeType,
                        FrameworkCode = l.FrameworkCode,
                        PathwayCode = l.PathwayCode,
                        NegotiatedPrice = l.NegotiatedPrice,
                        LearningStartDate = l.LearnStartDate,
                        PriceEpisodeIdentifier = l.PriceEpisodeIdentifier
                    }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetProviderLearnersQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}
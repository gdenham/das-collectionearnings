using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.GetDataLockFailuresQuery
{
    public class GetDataLockFailuresQueryHandler : IRequestHandler<GetDataLockFailuresQueryRequest, GetDataLockFailuresQueryResponse>
    {
        public GetDataLockFailuresQueryResponse Handle(GetDataLockFailuresQueryRequest message)
        {
            try
            {
                // 1. UKPRN match
                var learnersWithNoUkprnMatch = message.DasLearners
                    .Where(
                        dl =>
                            !message.Commitments.Any(
                                c => UkprnMatch(c, dl)))
                    .ToList();

                var remainingLearners = GetRemainingLearners(message.DasLearners, learnersWithNoUkprnMatch);

                // 2. ULN match
                var learnersWithNoUlnMatch = remainingLearners
                    .Where(
                        dl =>
                            !message.Commitments.Any(
                                c => UlnMatch(c, dl)))
                    .ToList();

                remainingLearners = GetRemainingLearners(remainingLearners, learnersWithNoUlnMatch);

                // 3.1. Standard match (if standard is present)
                var learnersWithNoStandardMatch = remainingLearners
                    .Where(
                        dl =>
                            dl.StdCode.HasValue &&
                            !message.Commitments.Any(
                                c => StandardMatch(c, dl)))
                    .ToList();

                remainingLearners = GetRemainingLearners(remainingLearners, learnersWithNoStandardMatch);

                // 3.2. Framework match (if standard is not present)
                var learnersWithNoFrameworkMatch = remainingLearners
                    .Where(
                        dl =>
                            !dl.StdCode.HasValue &&
                            !message.Commitments.Any(
                                c => FrameworkMatch(c, dl)))
                    .ToList();

                remainingLearners = GetRemainingLearners(remainingLearners, learnersWithNoFrameworkMatch);

                // 3.3. Programme match (if standard is not present)
                var learnersWithNoProgrammeMatch = remainingLearners
                    .Where(
                        dl =>
                            !dl.StdCode.HasValue &&
                            !message.Commitments.Any(
                                c => ProgrammeMatch(c, dl)))
                    .ToList();

                remainingLearners = GetRemainingLearners(remainingLearners, learnersWithNoProgrammeMatch);

                // 3.4. Pathway match (if standard is not present)
                var learnersWithNoPathwayMatch = remainingLearners
                    .Where(
                        dl =>
                            !dl.StdCode.HasValue &&
                            !message.Commitments.Any(
                                c => PathwayMatch(c, dl)))
                    .ToList();

                remainingLearners = GetRemainingLearners(remainingLearners, learnersWithNoPathwayMatch);

                // 4. Price match
                var learnersWithNoPriceMatch = remainingLearners
                    .Where(
                        dl =>
                            !dl.StdCode.HasValue &&
                            !message.Commitments.Any(
                                c => PriceMatch(c, dl)))
                    .ToList();

                // 5. Union data lock errors
                var errors = learnersWithNoUkprnMatch
                    .Select(
                        dl =>
                            new Data.Entities.ValidationError
                            {
                                LearnRefNumber = dl.LearnRefNumber,
                                AimSeqNumber = dl.AimSeqNumber,
                                RuleId = "DLOCK_01"
                            })
                    .Union(learnersWithNoUlnMatch
                        .Select(
                            dl =>
                                new Data.Entities.ValidationError
                                {
                                    LearnRefNumber = dl.LearnRefNumber,
                                    AimSeqNumber = dl.AimSeqNumber,
                                    RuleId = "DLOCK_02"
                                })
                    )
                    .Union(learnersWithNoStandardMatch
                        .Select(
                            dl =>
                                new Data.Entities.ValidationError
                                {
                                    LearnRefNumber = dl.LearnRefNumber,
                                    AimSeqNumber = dl.AimSeqNumber,
                                    RuleId = "DLOCK_03"
                                })
                    )
                    .Union(learnersWithNoFrameworkMatch
                        .Select(
                            dl =>
                                new Data.Entities.ValidationError
                                {
                                    LearnRefNumber = dl.LearnRefNumber,
                                    AimSeqNumber = dl.AimSeqNumber,
                                    RuleId = "DLOCK_04"
                                })
                    )
                    .Union(learnersWithNoProgrammeMatch
                        .Select(
                            dl =>
                                new Data.Entities.ValidationError
                                {
                                    LearnRefNumber = dl.LearnRefNumber,
                                    AimSeqNumber = dl.AimSeqNumber,
                                    RuleId = "DLOCK_05"
                                })
                    )
                    .Union(learnersWithNoPathwayMatch
                        .Select(
                            dl =>
                                new Data.Entities.ValidationError
                                {
                                    LearnRefNumber = dl.LearnRefNumber,
                                    AimSeqNumber = dl.AimSeqNumber,
                                    RuleId = "DLOCK_06"
                                })
                    )
                    .Union(learnersWithNoPriceMatch
                        .Select(
                            dl =>
                                new Data.Entities.ValidationError
                                {
                                    LearnRefNumber = dl.LearnRefNumber,
                                    AimSeqNumber = dl.AimSeqNumber,
                                    RuleId = "DLOCK_07"
                                })
                    );


                return new GetDataLockFailuresQueryResponse
                {
                    IsValid = true,
                    Items = errors
                };
            }
            catch (Exception ex)
            {
                return new GetDataLockFailuresQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }

        private List<Data.Entities.DasLearner> GetRemainingLearners(IEnumerable<Data.Entities.DasLearner> learners, IEnumerable<Data.Entities.DasLearner> learnersToRemove)
        {
            return learners.Except(learnersToRemove).ToList();
        } 

        private bool UkprnMatch(Data.Entities.Commitment commitment, Data.Entities.DasLearner dasLearner)
        {
            return commitment.Ukprn == dasLearner.Ukprn;
        }

        private bool UlnMatch(Data.Entities.Commitment commitment, Data.Entities.DasLearner dasLearner)
        {
            return dasLearner.Uln.HasValue && commitment.Uln == dasLearner.Uln.Value;
        }

        private bool StandardMatch(Data.Entities.Commitment commitment, Data.Entities.DasLearner dasLearner)
        {
            return commitment.StandardCode.HasValue &&
                   dasLearner.StdCode.HasValue &&
                   commitment.StandardCode.Value == dasLearner.StdCode.Value;
        }

        private bool FrameworkMatch(Data.Entities.Commitment commitment, Data.Entities.DasLearner dasLearner)
        {
            return commitment.FrameworkCode.HasValue &&
                   dasLearner.FworkCode.HasValue &&
                   commitment.FrameworkCode.Value == dasLearner.FworkCode.Value;
        }

        private bool ProgrammeMatch(Data.Entities.Commitment commitment, Data.Entities.DasLearner dasLearner)
        {
            return commitment.ProgrammeType.HasValue &&
                   dasLearner.ProgType.HasValue &&
                   commitment.ProgrammeType.Value == dasLearner.ProgType.Value;
        }

        private bool PathwayMatch(Data.Entities.Commitment commitment, Data.Entities.DasLearner dasLearner)
        {
            return commitment.PathwayCode.HasValue &&
                   dasLearner.PwayCode.HasValue &&
                   commitment.PathwayCode.Value == dasLearner.PwayCode.Value;
        }

        private bool PriceMatch(Data.Entities.Commitment commitment, Data.Entities.DasLearner dasLearner)
        {
            return dasLearner.TbFinAmount.HasValue &&
                   (long) commitment.AgreedCost == dasLearner.TbFinAmount.Value;
        }
    }
}
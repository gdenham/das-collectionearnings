using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.GetDataLockFailuresQuery
{
    public class GetDataLockFailuresQueryHandler : IRequestHandler<GetDataLockFailuresQueryRequest, GetDataLockFailuresQueryResponse>
    {
        public GetDataLockFailuresQueryResponse Handle(GetDataLockFailuresQueryRequest message)
        {
            try
            {
                var validationErrors = new ConcurrentBag<Data.Entities.ValidationError>();

                var learners = message.DasLearners.ToList();
                var partitioner = Partitioner.Create(0, learners.Count);

                Parallel.ForEach(partitioner, range =>
                {
                    for (var x = range.Item1; x < range.Item2; x++)
                    {
                        var dl = learners[x];

                        var gotMismatch = false;

                        // 1. UKPRN match
                        if (!message.Commitments.Any(c => DataLockMatcher.Match(c, dl, DataLockMatchLevel.Ukprn)))
                        {
                            validationErrors.Add(new Data.Entities.ValidationError
                            {
                                LearnRefNumber = dl.LearnRefNumber,
                                AimSeqNumber = dl.AimSeqNumber,
                                RuleId = "DLOCK_01"
                            });

                            gotMismatch = true;
                        }

                        // 2. ULN match
                        var learnerCommitments = message.Commitments.Where(c => dl.Uln.HasValue && c.Uln == dl.Uln.Value).ToList();

                        if (!gotMismatch &&
                            !learnerCommitments.Any(c => DataLockMatcher.Match(c, dl, DataLockMatchLevel.Uln)))
                        {
                            validationErrors.Add(new Data.Entities.ValidationError
                            {
                                LearnRefNumber = dl.LearnRefNumber,
                                AimSeqNumber = dl.AimSeqNumber,
                                RuleId = "DLOCK_02"
                            });

                            gotMismatch = true;
                        }

                        if (!gotMismatch)
                        {
                            if (dl.StdCode.HasValue)
                            {
                                // standard
                                // 3.1. Standard match (if standard is present)
                                if (!learnerCommitments.Any(c => DataLockMatcher.Match(c, dl, DataLockMatchLevel.Standard)))
                                {
                                    validationErrors.Add(new Data.Entities.ValidationError
                                    {
                                        LearnRefNumber = dl.LearnRefNumber,
                                        AimSeqNumber = dl.AimSeqNumber,
                                        RuleId = "DLOCK_03"
                                    });

                                    gotMismatch = true;
                                }
                            }
                            else
                            {
                                // framework
                                // 3.2. Framework match (if standard is not present)
                                if (!learnerCommitments.Any(c => DataLockMatcher.Match(c, dl, DataLockMatchLevel.Framework)))
                                {
                                    validationErrors.Add(new Data.Entities.ValidationError
                                    {
                                        LearnRefNumber = dl.LearnRefNumber,
                                        AimSeqNumber = dl.AimSeqNumber,
                                        RuleId = "DLOCK_04"
                                    });

                                    gotMismatch = true;
                                }

                                // 3.3. Programme match (if standard is not present)
                                if (!gotMismatch &&
                                    !learnerCommitments.Any(c => DataLockMatcher.Match(c, dl, DataLockMatchLevel.Programme)))
                                {
                                    validationErrors.Add(new Data.Entities.ValidationError
                                    {
                                        LearnRefNumber = dl.LearnRefNumber,
                                        AimSeqNumber = dl.AimSeqNumber,
                                        RuleId = "DLOCK_05"
                                    });

                                    gotMismatch = true;
                                }

                                // 3.4. Pathway match (if standard is not present)
                                if (!gotMismatch &&
                                    !learnerCommitments.Any(c => DataLockMatcher.Match(c, dl, DataLockMatchLevel.Pathway)))
                                {
                                    validationErrors.Add(new Data.Entities.ValidationError
                                    {
                                        LearnRefNumber = dl.LearnRefNumber,
                                        AimSeqNumber = dl.AimSeqNumber,
                                        RuleId = "DLOCK_06"
                                    });

                                    gotMismatch = true;
                                }
                            }

                            // 4. Price match
                            if (!gotMismatch &&
                                !learnerCommitments.Any(c => DataLockMatcher.Match(c, dl, DataLockMatchLevel.Price)))
                            {
                                validationErrors.Add(new Data.Entities.ValidationError
                                {
                                    LearnRefNumber = dl.LearnRefNumber,
                                    AimSeqNumber = dl.AimSeqNumber,
                                    RuleId = "DLOCK_07"
                                });
                            }
                        }
                    }
                });

                return new GetDataLockFailuresQueryResponse
                {
                    IsValid = true,
                    Items = validationErrors
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
    }
}
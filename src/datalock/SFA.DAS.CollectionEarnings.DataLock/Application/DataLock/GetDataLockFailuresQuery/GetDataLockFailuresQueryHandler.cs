﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;

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
                        var learner = learners[x];

                        var ukprnMatcher = new UkprnMatchHandler();
                        var ulnMatcher = new UlnMatchHandler();
                        var standardMatcher = new StandardMatchHandler();
                        var frameworkMatcher = new FrameworkMatchHandler();
                        var programmeMatcher = new ProgrammeMatchHandler();
                        var pathwaymatcher = new PathwayMatchHandler();
                        var priceMatcher = new PriceMatchHandler();
                        var multipleMatcher = new MultipleMatchHandler();

                        // Setup matching chain
                        ukprnMatcher.SetNextMatchHandler(ulnMatcher);

                        if (learner.StdCode.HasValue)
                        {
                            ulnMatcher.SetNextMatchHandler(standardMatcher);
                            standardMatcher.SetNextMatchHandler(priceMatcher);
                        }
                        else
                        {
                            ulnMatcher.SetNextMatchHandler(frameworkMatcher);
                            frameworkMatcher.SetNextMatchHandler(programmeMatcher);
                            programmeMatcher.SetNextMatchHandler(pathwaymatcher);
                            pathwaymatcher.SetNextMatchHandler(priceMatcher);
                        }

                        priceMatcher.SetNextMatchHandler(multipleMatcher);

                        // Execute the matching chain
                        var matchResult = ukprnMatcher.Match(message.Commitments.ToList(), learner);

                        if (!string.IsNullOrEmpty(matchResult))
                        {
                            validationErrors.Add(new Data.Entities.ValidationError
                            {
                                LearnRefNumber = learner.LearnRefNumber,
                                AimSeqNumber = learner.AimSeqNumber,
                                RuleId = matchResult
                            });
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
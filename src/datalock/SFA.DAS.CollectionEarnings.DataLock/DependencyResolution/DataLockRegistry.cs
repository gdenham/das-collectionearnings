using System;
using MediatR;
using NLog;
using SFA.DAS.CollectionEarnings.DataLock.Data.Repositories;
using StructureMap;

namespace SFA.DAS.CollectionEarnings.DataLock.DependencyResolution
{
    public class DataLockRegistry : Registry
    {
        public DataLockRegistry(Type taskType)
        {
            Scan(
                scan =>
                {
                    scan.AssemblyContainingType<DataLockRegistry>();

                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                });

            For<IMediator>().Use<Mediator>();

            // TODO: Fix so can be registered with convention
            For<ICommitmentRepository>().Use<CommitmentRepository>();
            For<IDasLearnerRepository>().Use<DasLearnerRepository>();
            For<IValidationErrorRepository>().Use<ValidationErrorRepository>();

            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
        }
    }
}
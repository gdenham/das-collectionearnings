using System;
using MediatR;
using NLog;
using StructureMap;

namespace SFA.DAS.CollectionEarnings.Calculator.DependencyResolution
{
    public class CalculatorRegistry : Registry
    {
        private const string ServiceName = "SFA.DAS.CollectionEarnings.Calculator";

        public CalculatorRegistry(Type taskType)
        {
            Scan(
                scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith(ServiceName));

                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                });

            For<IMediator>().Use<Mediator>();

            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
        }
    }
}

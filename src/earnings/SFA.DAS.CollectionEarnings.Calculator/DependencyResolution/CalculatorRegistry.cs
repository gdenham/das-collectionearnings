using System;
using MediatR;
using NLog;
using SFA.DAS.CollectionEarnings.Calculator.Data.Repositories;
using SFA.DAS.CollectionEarnings.Calculator.Tools.Providers;
using StructureMap;

namespace SFA.DAS.CollectionEarnings.Calculator.DependencyResolution
{
    public class CalculatorRegistry : Registry
    {
        public CalculatorRegistry(Type taskType)
        {
            Scan(
                scan =>
                {
                    scan.AssemblyContainingType<CalculatorRegistry>();

                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                });

            For<IMediator>().Use<Mediator>();

            // TODO: Fix so can be registered with convention
            For<ILearningDeliveryToProcessRepository>().Use<LearningDeliveryToProcessRepository>();
            For<IProcessedLearningDeliveryRepository>().Use<ProcessedLearningDeliveryRepository>();
            For<IProcessedLearningDeliveryPeriodisedValuesRepository>().Use<ProcessedLearningDeliveryPeriodisedValuesRepository>();
            For<IDateTimeProvider>().Use<DateTimeProvider>();

            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
        }
    }
}

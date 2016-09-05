﻿using System;
using MediatR;
using NLog;
using SFA.DAS.CollectionEarnings.Calculator.Context;
using StructureMap;

namespace SFA.DAS.CollectionEarnings.Calculator.DependencyResolution
{
    public class TaskDependencyResolver : IDependencyResolver
    {
        private IContainer _container;

        private const string ServiceName = "SFA.DAS.CollectionEarnings.Calculator";

        public void Init(Type taskType, ContextWrapper contextWrapper)
        {
            _container = new Container(c =>
                {
                    c.For<ILogger>()
                        .Use(() => LogManager.GetLogger(taskType.FullName));

                    c.Scan(scn =>
                        {
                            scn.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith(ServiceName));

                            scn.RegisterConcreteTypesAgainstTheFirstInterface();
                        }
                    );

                    c.For<SingleInstanceFactory>()
                        .Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
                    c.For<MultiInstanceFactory>()
                        .Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
                    c.For<IMediator>()
                        .Use<Mediator>();
                }
            );
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }
    }
}
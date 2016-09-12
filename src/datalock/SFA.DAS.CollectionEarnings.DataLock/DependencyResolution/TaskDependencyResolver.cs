﻿using System;
using SFA.DAS.CollectionEarnings.DataLock.Context;
using StructureMap;

namespace SFA.DAS.CollectionEarnings.DataLock.DependencyResolution
{
    public class TaskDependencyResolver : IDependencyResolver
    {
        private IContainer _container;

        public void Init(Type taskType, ContextWrapper contextWrapper)
        {
            _container = new Container(c =>
                {
                    c.Policies.Add(new ConnectionStringPolicy(contextWrapper));
                    c.AddRegistry(new DataLockRegistry(taskType));
                }
            );
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }
    }
}

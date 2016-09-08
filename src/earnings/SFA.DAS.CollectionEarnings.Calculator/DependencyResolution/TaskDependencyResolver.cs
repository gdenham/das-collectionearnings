using System;
using SFA.DAS.CollectionEarnings.Calculator.Context;
using StructureMap;

namespace SFA.DAS.CollectionEarnings.Calculator.DependencyResolution
{
    public class TaskDependencyResolver : IDependencyResolver
    {
        private IContainer _container;

        public void Init(Type taskType, ContextWrapper contextWrapper)
        {
            _container = new Container(c =>
                {
                    c.Policies.Add(new ConnectionStringPolicy(contextWrapper));
                    c.Policies.Add(new YearOfCollectionPolicy(contextWrapper));
                    c.AddRegistry(new CalculatorRegistry(taskType));
                }
            );
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }
    }
}

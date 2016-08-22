using System;
using NLog;
using SFA.DAS.CollectionEarnings.DataLock.Context;
using SFA.DAS.CollectionEarnings.DataLock.Data.Repositories;
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
                    c.For<ILogger>()
                        .Use(() => LogManager.GetLogger(taskType.FullName));

                    c.For<IValidationErrorRepository>()
                        .Use<ValidationErrorRepository>()
                        .Ctor<string>()
                        .Is(contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString));
                }
            );
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }
    }
}

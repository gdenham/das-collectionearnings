using System;

namespace SFA.DAS.CollectionEarnings.DataLock.DependencyResolution
{
    public interface IDependencyResolver
    {
        void Init(Type taskType);

        T GetInstance<T>();
    }
}

using System;
using SFA.DAS.CollectionEarnings.DataLock.Context;

namespace SFA.DAS.CollectionEarnings.DataLock.DependencyResolution
{
    public interface IDependencyResolver
    {
        void Init(Type taskType, ContextWrapper contextWrapper);

        T GetInstance<T>();
    }
}

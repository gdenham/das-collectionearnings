using System;

namespace SFA.DAS.CollectionEarnings.Domain.DependencyResolution
{
    public interface IDependencyResolver
    {
        void Init(Type taskType);

        T GetInstance<T>();
    }
}

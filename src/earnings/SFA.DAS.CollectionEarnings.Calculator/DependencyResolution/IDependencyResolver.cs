using System;
using SFA.DAS.CollectionEarnings.Calculator.Context;

namespace SFA.DAS.CollectionEarnings.Calculator.DependencyResolution
{
    public interface IDependencyResolver
    {
        void Init(Type taskType, ContextWrapper contextWrapper);

        T GetInstance<T>();
    }
}

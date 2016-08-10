using System.Collections.Generic;

namespace SFA.DAS.CollectionEarnings.Contract
{
    public interface IExternalTask
    {
        void Execute(IDictionary<string, string> context);
    }
}

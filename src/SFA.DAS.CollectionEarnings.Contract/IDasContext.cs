using System.Collections.Generic;

namespace SFA.DAS.CollectionEarnings.Contract
{
    public interface IDasContext
    {
        Dictionary<string, string> Properties { get; set; }
    }
}

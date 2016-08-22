using CS.Common.External.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.CollectionEarnings.DataLock.Common.Tests.ExternalContext
{
    public class ExternalContextStub : IExternalContext
    {
        public IDictionary<string, string> Properties { get; set; }
    }
}

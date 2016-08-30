using System;
using System.Collections.Generic;

namespace SFA.DAS.CollectionEarnings.DataLock.Application
{
    public abstract class QueryResponse<T>
    {
        public bool IsValid { get; set; }
        public Exception Exception { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
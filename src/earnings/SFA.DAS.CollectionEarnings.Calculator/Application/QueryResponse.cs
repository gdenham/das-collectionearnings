using System;
using System.Collections.Generic;

namespace SFA.DAS.CollectionEarnings.Calculator.Application
{
    public abstract class QueryResponse<T> : Response
    {
        public IEnumerable<T> Items { get; set; }
    }
}
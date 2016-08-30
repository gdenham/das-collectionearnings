using System;

namespace SFA.DAS.CollectionEarnings.DataLock.Application
{
    public abstract class CommandResponse
    {
        public bool IsValid { get; set; }
        public Exception Exception { get; set; }
    }
}

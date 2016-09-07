using System;

namespace SFA.DAS.CollectionEarnings.Calculator.Application
{
    public abstract class CommandResponse
    {
        public bool IsValid { get; set; }
        public Exception Exception { get; set; }
    }
}
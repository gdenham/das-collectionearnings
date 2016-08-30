using System;

namespace SFA.DAS.CollectionEarnings.DataLock.Exceptions
{
    public class DataLockProcessorException : Exception
    {
        public DataLockProcessorException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
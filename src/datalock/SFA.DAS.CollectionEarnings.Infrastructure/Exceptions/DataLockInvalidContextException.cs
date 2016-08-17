using System;

namespace SFA.DAS.CollectionEarnings.Infrastructure.Exceptions
{
    public class DataLockInvalidContextException : Exception
    {
        public DataLockInvalidContextException(string message)
            : base(message)
        {
        }

        public DataLockInvalidContextException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}

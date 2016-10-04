using System;

namespace SFA.DAS.CollectionEarnings.DataLock
{
    public class DataLockProcessorException : Exception
    {
        public const string ErrorReadingCommitmentsMessage = "Error while reading commitments.";
        public const string ErrorReadingLearnersMessage = "Error while reading DAS specific learners.";
        public const string ErrorPerformingDataLockMessage = "Error while performing data lock.";
        public const string ErrorWritingDataLockValidationErrorsMessage = "Error while writing data lock validation errors.";
        public const string ErrorWritingMatchingLearnersAndCommitmentsMessage = "Error while writing matching learners and commitments.";

        public DataLockProcessorException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
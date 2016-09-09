namespace SFA.DAS.CollectionEarnings.DataLock.Exceptions
{
    public class DataLockExceptionMessages
    {
        public const string ContextNull = "The context is null.";
        public const string ContextNoProperties = "The context contains no properties.";
        public const string ContextPropertiesNoConnectionString = "The context does not contain the transient database connection string property.";
        public const string ContextPropertiesNoLogLevel = "The context does not contain the logging level property.";
        public const string ContextPropertiesInvalidLogLevel = "The context does not contain a valid logging level.";

        public const string ErrorReadingCommitments = "Error while reading commitments.";
        public const string ErrorReadingDasLearners = "Error while reading DAS specific learners.";
        public const string ErrorPerformingDataLock = "Error while performing data lock.";
        public const string ErrorWritingDataLockValidationErrors = "Error while writing data lock validation errors.";
    }
}

namespace SFA.DAS.CollectionEarnings.Calculator.Exceptions
{
    public class EarningsCalculatorExceptionMessages
    {
        public const string ContextNull = "The context is null.";
        public const string ContextNoProperties = "The context contains no properties.";
        public const string ContextPropertiesNoConnectionString = "The context does not contain the transient database connection string property.";
        public const string ContextPropertiesNoLogLevel = "The context does not contain the logging level property.";
        public const string ContextPropertiesInvalidLogLevel = "The context does not contain a valid logging level.";
        public const string ContextPropertiesNoYearOfCollection = "The context does not contain the year of collection property.";
        public const string ContextPropertiesInvalidYearOfCollection = "The context does not contain a valid year of collection property.";

        public const string ErrorReadingLearningDeliveriesToProcess = "Error while reading learning deliveries to process.";
        public const string ErrorCalculatingEarningsForTheLearningDeliveries = "Error while processing the learning deliveries to calculate the earnings.";
        public const string ErrorWritingProcessedLearningDeliveries = "Error while writing processed learning deliveries.";
        public const string ErrorWritingProcessedLearningDeliveryPeriodisedValues = "Error while writing processed learning deliveries periodised values.";
    }
}

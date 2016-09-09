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
    }
}

using System;

namespace SFA.DAS.CollectionEarnings.Calculator.Exceptions
{
    public class EarningsCalculatorProcessorException : Exception
    {
        public EarningsCalculatorProcessorException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
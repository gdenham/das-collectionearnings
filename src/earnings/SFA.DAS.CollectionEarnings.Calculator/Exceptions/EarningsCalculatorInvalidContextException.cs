using System;

namespace SFA.DAS.CollectionEarnings.Calculator.Exceptions
{
    public class EarningsCalculatorInvalidContextException : Exception
    {
        public EarningsCalculatorInvalidContextException(string message)
            : base(message)
        {
        }

        public EarningsCalculatorInvalidContextException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}

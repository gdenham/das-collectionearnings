using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Exceptions;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Context.ContextWrapper.ContextWrapper
{
    public class WhenCalledWithInvalidContext
    {
        private static readonly object[] EmptyProperties =
        {
            new object[] {null},
            new object[] {new Dictionary<string, string>()}
        };

        [Test]
        public void ThenExpectingExceptionForNullContextProvided()
        {
            // Assert
            // ReSharper disable once ObjectCreationAsStatement
            var ex = Assert.Throws<EarningsCalculatorInvalidContextException>(() => new Calculator.Context.ContextWrapper(null));
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorExceptionMessages.ContextNull));
        }

        [Test]
        [TestCaseSource(nameof(EmptyProperties))]
        public void ThenExpectingExceptionForNoContextPropertiesProvided(IDictionary<string, string> properties)
        {
            // Arrange
            var context = new ExternalContextStub { Properties = properties };

            // Assert
            // ReSharper disable once ObjectCreationAsStatement
            var ex = Assert.Throws<EarningsCalculatorInvalidContextException>(() => new Calculator.Context.ContextWrapper(context));
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorExceptionMessages.ContextNoProperties));
        }
    }
}

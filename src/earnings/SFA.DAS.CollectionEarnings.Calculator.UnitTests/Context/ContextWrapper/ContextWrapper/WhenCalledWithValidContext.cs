﻿using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Context.ContextWrapper.ContextWrapper
{
    public class WhenCalledWithValidContext
    {
        public void ThenExpectingInstance()
        {
            // Arrange
            var context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {"key", "value"}
                }
            };

            // Act
            var contextWrapper = new Calculator.Context.ContextWrapper(context);

            // Assert
            Assert.IsNotNull(contextWrapper);
        }
    }
}

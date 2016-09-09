﻿using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Context.ContextWrapper.GetPropertyValue
{
    public class WhenCalledForUnknownProperty
    {
        private Calculator.Context.ContextWrapper _contextWrapper;

        [SetUp]
        public void Arrange()
        {
            var context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {"key", "value"}
                }
            };

            _contextWrapper = new Calculator.Context.ContextWrapper(context);
        }

        [Test]
        public void ThenDefaultValueReturnedWhenDefaultValueProvided()
        {
            // Act
            var defaultVal = "defaultvalue";
            var val = _contextWrapper.GetPropertyValue("unknownkey", defaultVal);

            // Assert
            Assert.AreEqual(defaultVal, val);
        }

        [Test]
        public void ThenNullValueReturnedWhenNoDefaultValueProvided()
        {
            // Act
            var val = _contextWrapper.GetPropertyValue("unknownkey");

            // Assert
            Assert.IsNull(val);
        }
    }
}

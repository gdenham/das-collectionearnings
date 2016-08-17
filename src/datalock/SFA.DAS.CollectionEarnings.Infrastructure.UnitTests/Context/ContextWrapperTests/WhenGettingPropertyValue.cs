using System.Collections.Generic;
using CS.Common.External.Interfaces;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Infrastructure.Context;

/*
 * Unit tests pattern under review. Might be changed in other solutions. Should not be taken as reference.
 */
//TODO Change test format or remove comments
namespace SFA.DAS.CollectionEarnings.Infrastructure.UnitTests.Context.ContextWrapperTests
{
    public class WhenGettingPropertyValue
    {
        private ContextWrapper _contextWrapper;

        [SetUp]
        public void Arrange()
        {
            var context = new ExternalContext
            {
                Properties = new Dictionary<string, string>
                {
                    {"key", "value"}
                }
            };

            _contextWrapper = new ContextWrapper(context);
        }

        [Test]
        public void ThenCorrectValueForKnownProperty()
        {
            // Act
            var val = _contextWrapper.GetPropertyValue("key");

            // Assert
            Assert.AreEqual("value", val);
        }

        [Test]
        public void ThenDefaultValueForUnknownPropertyWithDefaultValue()
        {
            // Act
            var defaultVal = "defaultvalue";
            var val = _contextWrapper.GetPropertyValue("unknownkey", defaultVal);

            // Assert
            Assert.AreEqual(defaultVal, val);
        }

        [Test]
        public void ThenNullForUnknownPropertyWithNoDefaultValue()
        {
            // Act
            var val = _contextWrapper.GetPropertyValue("unknownkey");

            // Assert
            Assert.IsNull(val);
        }

        private class ExternalContext : IExternalContext
        {
            public IDictionary<string, string> Properties { get; set; }
        }
    }
}

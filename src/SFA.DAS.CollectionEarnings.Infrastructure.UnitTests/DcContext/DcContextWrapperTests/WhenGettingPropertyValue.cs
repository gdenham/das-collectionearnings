using System.Collections.Generic;
using CS.Common.External.Interfaces;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Infrastructure.DcContext;

/*
 * Unit tests pattern under review. Might be changed in other solutions. Should not be taken as reference.
 */
//TODO Change test format or remove comments
namespace SFA.DAS.CollectionEarnings.Infrastructure.UnitTests.DcContext.DcContextWrapperTests
{
    public class WhenGettingPropertyValue
    {
        private DcContextWrapper _contextWrapper;

        [SetUp]
        public void Arrange()
        {
            var context = new ExternalContext();
            context.Properties = new Dictionary<string, string>
                {
                    { "key", "value" }
                };

            _contextWrapper = new DcContextWrapper(context);
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

        internal class ExternalContext : IExternalContext
        {
            public IDictionary<string, string> Properties { get; set; }
        }
    }
}

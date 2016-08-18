using System.Collections.Generic;
using CS.Common.External.Interfaces;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Context;
using SFA.DAS.CollectionEarnings.DataLock.Exceptions;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Context.ContextWrapperTests
{
    public class WhenInstantiating
    {
        private static readonly object[] EmptyProperties =
        {
            new object[] {null},
            new object[] {new Dictionary<string, string>()}
        };

        [Test]
        public void WithNullContextThenExpectingException()
        {
            // Assert
            // ReSharper disable once ObjectCreationAsStatement
            var ex = Assert.Throws<DataLockInvalidContextException>(() => new ContextWrapper(null));
            Assert.IsTrue(ex.Message.Contains(DataLockExceptionMessages.ContextNull));
        }

        [Test]
        [TestCaseSource(nameof(EmptyProperties))]
        public void WithNoContextPropertiesThenExpectingError(IDictionary<string, string> properties)
        {
            // Arrange
            var context = new ExternalContext {Properties = properties};

            // Assert
            // ReSharper disable once ObjectCreationAsStatement
            var ex = Assert.Throws<DataLockInvalidContextException>(() => new ContextWrapper(context));
            Assert.IsTrue(ex.Message.Contains(DataLockExceptionMessages.ContextNoProperties));
        }

        public void WithValidContextThenExpectingInstance()
        {
            // Arrange
            var context = new ExternalContext
            {
                Properties = new Dictionary<string, string>
                {
                    {"key", "value"}
                }
            };

            // Act
            var contextWrapper = new ContextWrapper(context);

            // Assert
            Assert.IsNotNull(contextWrapper);
        }

        private class ExternalContext : IExternalContext
        {
            public IDictionary<string, string> Properties { get; set; }
        }
    }
}

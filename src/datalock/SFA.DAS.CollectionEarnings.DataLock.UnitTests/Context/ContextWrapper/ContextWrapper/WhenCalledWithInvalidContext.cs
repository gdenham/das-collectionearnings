using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Exceptions;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Context.ContextWrapper.ContextWrapper
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
            var ex = Assert.Throws<DataLockInvalidContextException>(() => new DataLock.Context.ContextWrapper(null));
            Assert.IsTrue(ex.Message.Contains(DataLockExceptionMessages.ContextNull));
        }

        [Test]
        [TestCaseSource(nameof(EmptyProperties))]
        public void ThenExpectingExceptionForNoContextPropertiesProvided(IDictionary<string, string> properties)
        {
            // Arrange
            var context = new ExternalContextStub { Properties = properties };

            // Assert
            // ReSharper disable once ObjectCreationAsStatement
            var ex = Assert.Throws<DataLockInvalidContextException>(() => new DataLock.Context.ContextWrapper(context));
            Assert.IsTrue(ex.Message.Contains(DataLockExceptionMessages.ContextNoProperties));
        }
    }
}
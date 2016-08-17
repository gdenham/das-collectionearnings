using NLog;
using NLog.Layouts;
using NLog.Targets;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Infrastructure.Logging;
using System.Linq;
using SFA.DAS.CollectionEarnings.Infrastructure.Exceptions;

/*
 * Unit tests pattern under review. Might be changed in other solutions. Should not be taken as reference.
 */
 //TODO Change test format or remove comments
namespace SFA.DAS.CollectionEarnings.Infrastructure.UnitTests.Logging.LoggingConfigurationTests
{
    public class WhenConfiguringLogging
    {
        private readonly string _connectionString = "Ilr.Transient.Connection.String";
        private readonly string _logLevel = "Debug";
        private readonly string _invalidLogLevel = "Debug1";

        [Test]
        public void ThenSqlServerTargetIsPresentAndCorrectlyConfigured()
        {
            // Arrange
            LoggingConfig.ConfigureLogging(_connectionString, _logLevel);

            // Act
            var sqlServerTarget = (DatabaseTarget)LogManager.Configuration.FindTargetByName("sqlserver");

            // Assert
            Assert.IsNotNull(sqlServerTarget);
            Assert.AreEqual(_connectionString, ((SimpleLayout)sqlServerTarget.ConnectionString).OriginalText);
        }

        [Test]
        public void ThenSqlServerRuleIsPresentAndCorrectlyConfigured()
        {
            // Arrange
            LoggingConfig.ConfigureLogging(_connectionString, _logLevel);

            // Act
            var sqlServerRule = LogManager.Configuration.LoggingRules.FirstOrDefault(lr => lr.Targets.Count(t => t.Name == "sqlserver") == 1);

            // Assert
            Assert.IsNotNull(sqlServerRule);
            Assert.AreEqual(1, sqlServerRule.Levels.Count(lvl => lvl.Name == _logLevel));
        }

        [Test]
        public void ThenExpectingExceptionForInvalidLogLevel()
        {
            // Assert
            var ex = Assert.Throws<DataLockInvalidContextException>(() => LoggingConfig.ConfigureLogging(_connectionString, _invalidLogLevel));
            Assert.IsTrue(ex.Message.Contains(DataLockExceptionMessages.ContextPropertiesInvalidLogLevel));
        }
    }
}

using System.Linq;
using NLog;
using NLog.Layouts;
using NLog.Targets;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Exceptions;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Logging.LoggingConfig.ConfigureLogging
{
    public class WhenCalled
    {
        private readonly string _connectionString = "Ilr.Transient.Connection.String";
        private readonly string _logLevel = "Debug";
        private readonly string _invalidLogLevel = "Debug1";

        [Test]
        public void ThenSqlServerTargetIsPresentAndCorrectlyConfigured()
        {
            // Arrange
            Calculator.Logging.LoggingConfig.ConfigureLogging(_connectionString, _logLevel);

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
            Calculator.Logging.LoggingConfig.ConfigureLogging(_connectionString, _logLevel);

            // Act
            var sqlServerRule = LogManager.Configuration.LoggingRules.FirstOrDefault(lr => lr.Targets.Count(t => t.Name == "sqlserver") == 1);

            // Assert
            Assert.IsNotNull(sqlServerRule);
            Assert.AreEqual(1, sqlServerRule.Levels.Count(lvl => lvl.Name == _logLevel));
        }

        [Test]
        public void ThenExpectingExceptionForInvalidLogLevelProvided()
        {
            // Arrange
            Calculator.Logging.LoggingConfig.ConfigureLogging(_connectionString, _logLevel);

            // Assert
            var ex = Assert.Throws<EarningsCalculatorInvalidContextException>(() => Calculator.Logging.LoggingConfig.ConfigureLogging(_connectionString, _invalidLogLevel));
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorExceptionMessages.ContextPropertiesInvalidLogLevel));
        }
    }
}

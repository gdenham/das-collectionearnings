using NLog;
using NLog.Layouts;
using NLog.Targets;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Infrastructure.Logging;
using System.Linq;

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

        [SetUp]
        public void Arrange()
        {
            LoggingConfig.ConfigureLogging(_connectionString, _logLevel);
        }

        [Test]
        public void ThenSqlServerTargetIsPresentAndCorrectlyConfigured()
        {
            // Act
            var sqlServerTarget = (DatabaseTarget)LogManager.Configuration.FindTargetByName("sqlserver");

            // Assert
            Assert.IsNotNull(sqlServerTarget);
            Assert.AreEqual(_connectionString, ((SimpleLayout)sqlServerTarget.ConnectionString).OriginalText);
        }

        [Test]
        public void ThenSqlServerRuleIsPresentAndCorrectlyConfigured()
        {
            // Act
            var sqlServerRule = LogManager.Configuration.LoggingRules.Where(lr => lr.Targets.Count(t => t.Name == "sqlserver") == 1).FirstOrDefault();

            // Assert
            Assert.IsNotNull(sqlServerRule);
            Assert.AreEqual(1, sqlServerRule.Levels.Count(lvl => lvl.Name == _logLevel));
        }
    }
}

using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using NUnit.Framework;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests
{
    [SetUpFixture]
    public class GlobalSetup
    {
        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            SetupSubmissionDatabase();
            SetupPeriodEndDatabase();
        }

        private void SetupSubmissionDatabase()
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.SubmissionConnectionString))
            {
                connection.Open();

                try
                {
                    // Pre-req scripts
                    RunSqlScript(@"Ilr.Transient.DDL.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                    RunSqlScript(@"DasCommitments.deds.DDL.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);

                    // Component scripts
                    RunSqlScript(@"Ilr.Transient.Reference.DDL.Tables.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                    RunSqlScript(@"Ilr.Transient.DataLock.DDL.Tables.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                    RunSqlScript(@"Ilr.Transient.DataLock.DDL.Views.sql", connection, GlobalTestContext.Instance.BracketedSubmissionDatabaseName);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void SetupPeriodEndDatabase()
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.PeriodEndConnectionString))
            {
                connection.Open();

                try
                {
                    // Pre-req scripts
                    RunSqlScript(@"Ilr.Deds.DDL.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"Summarisation.Deds.DDL.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"DasCommitments.deds.DDL.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);

                    // Component scripts
                    RunSqlScript(@"PeriodEnd.Transient.DataLock.Reference.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);

                    RunSqlScript(@"PeriodEnd.Transient.Reference.CollectionPeriods.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"PeriodEnd.Transient.Reference.Commitments.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"PeriodEnd.Transient.Reference.Providers.ddl.tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);

                    RunSqlScript(@"PeriodEnd.Transient.DataLock.DDL.Tables.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                    RunSqlScript(@"PeriodEnd.Transient.DataLock.DDL.Views.sql", connection, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void RunSqlScript(string fileName, SqlConnection connection, string databaseName)
        {
            var path = Path.Combine(GlobalTestContext.Instance.AssemblyDirectory, "DbSetupScripts", fileName);
            var sql = ReplaceSqlTokens(File.ReadAllText(path), databaseName);
            var commands = sql.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var command in commands)
            {
                connection.Execute(command);
            }
        }

        private string ReplaceSqlTokens(string sql, string databaseName)
        {
            return sql.Replace("${ILR_Deds.FQ}", databaseName)
                      .Replace("${ILR_Summarisation.FQ}", databaseName)
                      .Replace("${DAS_Commitments.FQ}", databaseName)
                      .Replace("${DAS_PeriodEnd.FQ}", databaseName);
        }
    }
}
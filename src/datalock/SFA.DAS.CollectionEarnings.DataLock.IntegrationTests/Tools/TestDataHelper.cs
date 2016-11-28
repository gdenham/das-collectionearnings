using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Dapper;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools
{
    public class TestDataHelper
    {
        private static readonly string[] PeriodEndCopyReferenceDataScripts =
        {
            "01 PeriodEnd.Populate.Reference.CollectionPeriods.dml.sql",
            "02 PeriodEnd.Populate.Reference.Providers.dml.sql",
            "03 PeriodEnd.Populate.Reference.Commitments.dml.sql",
            "05 PeriodEnd.DataLock.Populate.Reference.Learners.dml.sql"
        };

        private static readonly string[] SubmissionCopyReferenceDataScripts =
        {
            "01 Ilr.Populate.Reference.CollectionPeriods.dml.sql",
            "02 Ilr.DataLock.Populate.Reference.DasCommitments.dml.sql"
        };

        internal static void Clean()
        {
            Clean(GlobalTestContext.Instance.SubmissionConnectionString);
            Clean(GlobalTestContext.Instance.SubmissionDedsConnectionString);
            Clean(GlobalTestContext.Instance.PeriodEndConnectionString);
            Clean(GlobalTestContext.Instance.PeriodEndDedsConnectionString);
        }

        private static void Clean(string connectionString)
        {
            Execute(connectionString,
                @"DECLARE @SQL NVARCHAR(MAX) = ''

                    SELECT @SQL = (
                        SELECT 'TRUNCATE TABLE [' + s.name + '].[' + o.name + ']' + CHAR(13)
                        FROM sys.objects o WITH (NOWAIT)
                        JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]
                        WHERE o.[type] = 'U'
                            AND s.name IN ('dbo', 'Input', 'Valid', 'Invalid', 'Reference', 'DataLock')
                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')

                    EXEC sys.sp_executesql @SQL                
                ");
        }

        internal static void AddCommitment(CommitmentEntity commitment)
        {
            AddCommitment(GlobalTestContext.Instance.SubmissionConnectionString, commitment);
        }

        internal static void PeriodEndAddCommitment(CommitmentEntity commitment)
        {
            AddCommitment(GlobalTestContext.Instance.PeriodEndConnectionString, commitment);
        }

        private static void AddCommitment(string connectionString, CommitmentEntity commitment)
        {
            Execute(connectionString,
                @"INSERT INTO [dbo].[DasCommitments] (
                        CommitmentId, 
                        Uln, 
                        Ukprn, 
                        AccountId, 
                        StartDate, 
                        EndDate, 
                        AgreedCost, 
                        StandardCode, 
                        ProgrammeType, 
                        FrameworkCode, 
                        PathwayCode,
                        Priority,
                        VersionId,
                        PaymentStatus,
                        PaymentStatusDescription,
                        Payable
                    ) VALUES (
                        @CommitmentId, 
                        @Uln, 
                        @Ukprn, 
                        @AccountId, 
                        @StartDate, 
                        @EndDate, 
                        @AgreedCost, 
                        @StandardCode, 
                        @ProgrammeType, 
                        @FrameworkCode, 
                        @PathwayCode,
                        1,
                        '1',
                        @PaymentStatus,
                        @PaymentStatusDescription,
                        @Payable
                    )",
                new
                {
                    CommitmentId = commitment.CommitmentId,
                    Uln = commitment.Uln,
                    Ukprn = commitment.Ukprn,
                    AccountId = commitment.AccountId,
                    StartDate = commitment.StartDate,
                    EndDate = commitment.EndDate,
                    AgreedCost = commitment.AgreedCost,
                    StandardCode = commitment.StandardCode,
                    ProgrammeType = commitment.ProgrammeType,
                    FrameworkCode = commitment.FrameworkCode,
                    PathwayCode = commitment.PathwayCode,
                    PaymentStatus = commitment.PaymentStatus,
                    PaymentStatusDescription = commitment.PaymentStatusDescription,
                    Payable = commitment.Payable
                });
        }

        internal static void AddProvider(long ukprn)
        {
            Execute(GlobalTestContext.Instance.SubmissionConnectionString,
                "INSERT INTO [Input].[LearningProvider] (LearningProvider_Id, UKPRN) VALUES (@id, @ukprn)",
                new
                {
                    id = ukprn,
                    ukprn = ukprn
                });
        }

        internal static void AddValidProvider(long ukprn, bool inDeds = false)
        {
            var connectionString = inDeds
                ? GlobalTestContext.Instance.SubmissionDedsConnectionString
                : GlobalTestContext.Instance.SubmissionConnectionString;

            Execute(connectionString,
                "INSERT INTO [Valid].[LearningProvider] (UKPRN) VALUES (@ukprn)",
                new
                {
                    ukprn = ukprn
                });
        }

        internal static void PeriodEndAddProvider(long ukprn)
        {
            Execute(GlobalTestContext.Instance.PeriodEndConnectionString,
                "INSERT INTO [Valid].[LearningProvider] (UKPRN) VALUES (@ukprn)",
                new
                {
                    ukprn = ukprn
                });


            Execute(GlobalTestContext.Instance.PeriodEndConnectionString,
                "INSERT INTO dbo.FileDetails (UKPRN, SubmittedTime) VALUES (@ukprn, @submittedTime)",
                new
                {
                    ukprn = ukprn,
                    submittedTime = DateTime.Today
                });
        }

        internal static void AddCollectionPeriod()
        {
            AddCollectionPeriod(GlobalTestContext.Instance.SubmissionConnectionString);
        }

        internal static void PeriodEndAddCollectionPeriod()
        {
            AddCollectionPeriod(GlobalTestContext.Instance.PeriodEndConnectionString);
        }

        private static void AddCollectionPeriod(string connectionString)
        {
            Execute(connectionString,
                "INSERT INTO [dbo].[Collection_Period_Mapping] ([Period_ID], [Collection_Period], [Period], [Calendar_Year], [Collection_Open], [ActualsSchemaPeriod]) VALUES (1, 'R01', 8, 2016, 1, 201608)");
        }

        internal static LearnerCommitmentEntity[] GetLearnerAndCommitmentMatches(bool inDeds = false)
        {
            var connectionString = inDeds
                ? GlobalTestContext.Instance.SubmissionDedsConnectionString
                : GlobalTestContext.Instance.SubmissionConnectionString;

            return GetLearnerAndCommitmentMatches(connectionString);
        }

        internal static LearnerCommitmentEntity[] PeriodEndGetLearnerAndCommitmentMatches(bool inDeds = false)
        {
            var connectionString = inDeds
                ? GlobalTestContext.Instance.PeriodEndDedsConnectionString
                : GlobalTestContext.Instance.PeriodEndConnectionString;

            return GetLearnerAndCommitmentMatches(connectionString);
        }

        private static LearnerCommitmentEntity[] GetLearnerAndCommitmentMatches(string connectionString)
        {
            return Query<LearnerCommitmentEntity>(connectionString, "SELECT * FROM [DataLock].[DasLearnerCommitment]");
        }

        internal static ValidationErrorEntity[] GetValidationErrors(bool inDeds = false)
        {
            var connectionString = inDeds
                ? GlobalTestContext.Instance.SubmissionDedsConnectionString
                : GlobalTestContext.Instance.SubmissionConnectionString;

            return GetValidationErrors(connectionString);
        }

        internal static ValidationErrorEntity[] PeriodEndGetValidationErrors(bool inDeds = false)
        {
            var connectionString = inDeds
                ? GlobalTestContext.Instance.PeriodEndDedsConnectionString
                : GlobalTestContext.Instance.PeriodEndConnectionString;

            return GetValidationErrors(connectionString);
        }

        private static ValidationErrorEntity[] GetValidationErrors(string connectionString)
        {
            return Query<ValidationErrorEntity>(connectionString, "SELECT * FROM [DataLock].[ValidationError]");
        }

        internal static void ExecuteScript(string script, bool inDeds = false)
        {
            var databaseName = inDeds
                ? GlobalTestContext.Instance.BracketedSubmissionDedsDatabaseName
                : GlobalTestContext.Instance.BracketedSubmissionDatabaseName;

            var sql = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Tools\Sql\{script}");

            ExecuteScript(
                GlobalTestContext.Instance.SubmissionConnectionString,
                databaseName,
                sql);
        }

        internal static void PeriodEndExecuteScript(string script, bool inDeds = false)
        {
            var databaseName = inDeds
                ? GlobalTestContext.Instance.BracketedPeriodEndDedsDatabaseName
                : GlobalTestContext.Instance.BracketedPeriodEndDatabaseName;

            var sql = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Tools\Sql\{script}");

            ExecuteScript(
                GlobalTestContext.Instance.PeriodEndConnectionString,
                databaseName,
                sql);
        }

        internal static void CopyReferenceData()
        {
            foreach (var script in SubmissionCopyReferenceDataScripts)
            {
                var sql = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Tools\Sql\Copy Reference Data\{script}");

                ExecuteScript(
                    GlobalTestContext.Instance.SubmissionConnectionString,
                    GlobalTestContext.Instance.BracketedSubmissionDatabaseName,
                    sql);
            }
        }

        internal static void PeriodEndCopyReferenceData()
        {
            foreach (var script in PeriodEndCopyReferenceDataScripts)
            {
                var sql = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Tools\Sql\Copy Reference Data\{script}");

                ExecuteScript(
                    GlobalTestContext.Instance.PeriodEndConnectionString,
                    GlobalTestContext.Instance.BracketedPeriodEndDatabaseName,
                    sql);
            }
        }

        private static void ExecuteScript(string connectionString, string databaseName, string sql)
        {
            var commands = ReplaceSqlTokens(sql, databaseName).Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var command in commands)
            {
                Execute(connectionString, command);
            }
        }

        private static void Execute(string connectionString, string command, object param = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    connection.Execute(command, param);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static T[] Query<T>(string connectionString, string command, object param = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    return connection.Query<T>(command, param)?.ToArray();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static string ReplaceSqlTokens(string sql, string databaseName)
        {
            return sql.Replace("${ILR_Deds.FQ}", databaseName)
                      .Replace("${ILR_Summarisation.FQ}", databaseName)
                      .Replace("${DAS_Commitments.FQ}", databaseName)
                      .Replace("${DAS_PeriodEnd.FQ}", databaseName)
                      .Replace("${YearOfCollection}", "1617");
        }
    }
}

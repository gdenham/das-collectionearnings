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
            "01 Ilr.DataLock.Populate.Reference.DasCommitments.dml.sql"
        };

        internal static void Clean()
        {
            Clean(GlobalTestContext.Instance.SubmissionConnectionString);
        }

        internal static void PeriodEndClean()
        {
            Clean(GlobalTestContext.Instance.PeriodEndConnectionString);
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
                        VersionId
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
                        '1'
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
                    PathwayCode = commitment.PathwayCode
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

        internal static LearnerCommitmentEntity[] GetLearnerAndCommitmentMatches()
        {
            return GetLearnerAndCommitmentMatches(GlobalTestContext.Instance.SubmissionConnectionString);
        }

        internal static LearnerCommitmentEntity[] PeriodEndGetLearnerAndCommitmentMatches()
        {
            return GetLearnerAndCommitmentMatches(GlobalTestContext.Instance.PeriodEndConnectionString);
        }

        private static LearnerCommitmentEntity[] GetLearnerAndCommitmentMatches(string connectionString)
        {
            return Query<LearnerCommitmentEntity>(connectionString, "SELECT * FROM [DataLock].[DasLearnerCommitment]");
        }

        internal static ValidationErrorEntity[] GetValidationErrors()
        {
            return GetValidationErrors(GlobalTestContext.Instance.SubmissionConnectionString);
        }

        internal static ValidationErrorEntity[] PeriodEndGetValidationErrors()
        {
            return GetValidationErrors(GlobalTestContext.Instance.PeriodEndConnectionString);
        }

        private static ValidationErrorEntity[] GetValidationErrors(string connectionString)
        {
            return Query<ValidationErrorEntity>(connectionString, "SELECT * FROM [DataLock].[ValidationError]");
        }

        internal static void PeriodEndExecuteScript(string script)
        {
            var scriptCommand = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Tools\Sql\{script}");

            Execute(GlobalTestContext.Instance.PeriodEndConnectionString, scriptCommand);
        }

        internal static void CopyReferenceData()
        {
            foreach (var script in SubmissionCopyReferenceDataScripts)
            {
                var sql = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Tools\Sql\Copy Reference Data\{script}");

                var commands = ReplaceSqlTokens(sql, GlobalTestContext.Instance.BracketedSubmissionDatabaseName).Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var command in commands)
                {
                    Execute(GlobalTestContext.Instance.SubmissionConnectionString, command);
                }
            }
        }

        internal static void PeriodEndCopyReferenceData()
        {
            foreach (var script in PeriodEndCopyReferenceDataScripts)
            {
                var sql = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Tools\Sql\Copy Reference Data\{script}");

                var commands = ReplaceSqlTokens(sql, GlobalTestContext.Instance.BracketedPeriodEndDatabaseName).Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var command in commands)
                {
                    Execute(GlobalTestContext.Instance.PeriodEndConnectionString, command);
                }
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
                      .Replace("${DAS_PeriodEnd.FQ}", databaseName);
        }
    }
}

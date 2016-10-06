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
                            AND s.name IN ('Input', 'Valid', 'Invalid', 'Reference', 'DataLock')
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
                @"INSERT INTO [Reference].[DasCommitments] (
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
                        PathwayCode
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
                        @PathwayCode
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

        internal static void AddProviderIlrSubmission(long ukprn)
        {
            Execute(GlobalTestContext.Instance.SubmissionConnectionString,
                "INSERT INTO [Input].[LearningProvider] (LearningProvider_Id, UKPRN) VALUES (@id, @ukprn)",
                new
                {
                    id = ukprn,
                    ukprn = ukprn
                });
        }

        internal static void AddProviderIlrPeriodEnd(long ukprn)
        {
            Execute(GlobalTestContext.Instance.PeriodEndConnectionString,
                "INSERT INTO [Valid].[LearningProvider] (UKPRN) VALUES (@ukprn)", 
                new
                {
                    ukprn = ukprn
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
    }
}

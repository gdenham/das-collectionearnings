﻿using System.Data.SqlClient;
using Dapper;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools
{
    public class Database
    {
        public static void Clean(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(@"
                    DECLARE @SQL NVARCHAR(MAX) = ''

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
        }

        public static void AddCommitment(string connectionString, CommitmentEntity commitment)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(@"
                    INSERT INTO [Reference].[DasCommitments] (
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
        }
    }
}

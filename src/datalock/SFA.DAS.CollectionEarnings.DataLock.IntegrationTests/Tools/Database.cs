using System.Data.SqlClient;
using Dapper;
using Dapper.Contrib.Extensions;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;

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

        public static void AddCommitment(string connectionString, Commitment commitment)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Insert(commitment);
            }
        }
    }
}

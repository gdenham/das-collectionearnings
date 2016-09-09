using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;

namespace SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Tools
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
                            AND s.name IN ('Input', 'Valid', 'Invalid', 'Earnings', 'Rulebase')
                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')

                    EXEC sys.sp_executesql @SQL                
                ");
            }
        }

        public static void AddIlrDataNoLearningDeliveriesToProcess(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Tools\Sql\IlrDataNoLearningDeliveriesToProcess.sql");

                connection.Execute(sql);
            }
        }

        public static void AddIlrDataOneLearningDeliveryToProcess(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Tools\Sql\IlrDataOneLearningDeliveryToProcess.sql");

                connection.Execute(sql);
            }
        }

        public static void AddIlrDataMultipleLearningDeliveriesToProcess(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Tools\Sql\IlrDataMultipleLearningDeliveriesToProcess.sql");

                connection.Execute(sql);
            }
        }
    }
}

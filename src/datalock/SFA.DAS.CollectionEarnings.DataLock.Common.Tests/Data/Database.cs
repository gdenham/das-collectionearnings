﻿using System.Data.SqlClient;
using Dapper;

namespace SFA.DAS.CollectionEarnings.DataLock.Common.Tests.Data
{
    public class Database
    {
        public static void Clean(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute("EXEC sp_MSForEachTable 'TRUNCATE TABLE ?'");
            }
        }
    }
}

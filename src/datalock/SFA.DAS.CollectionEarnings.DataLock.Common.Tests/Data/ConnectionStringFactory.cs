using System;
using System.Configuration;

namespace SFA.DAS.CollectionEarnings.DataLock.Common.Tests.Data
{
    public class ConnectionStringFactory
    {

        public static string GetTransientConnectionString()
        {
            var connectionString = "";

            if (Environment.GetEnvironmentVariable("TransientConnectionString") != null)
            {
                connectionString = Environment.GetEnvironmentVariable("TransientConnectionString");
            }
            else
            {
                connectionString = ConfigurationManager.AppSettings["TransientConnectionString"];
            }

            return connectionString;
        }
    }
}

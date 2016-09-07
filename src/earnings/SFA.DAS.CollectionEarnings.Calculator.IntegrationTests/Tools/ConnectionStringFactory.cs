using System;
using System.Configuration;

namespace SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Tools
{
    public class ConnectionStringFactory
    {

        public static string GetTransientConnectionString()
        {
            string connectionString;

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

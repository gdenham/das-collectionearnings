using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests
{
    internal class GlobalTestContext
    {
        private const string SubmissionConnectionStringKey = "SubmissionConnectionString";
        private const string PeriodEndConnectionStringKey = "PeriodEndConnectionString";

        private GlobalTestContext()
        {
            try
            {
                SetupConnectionStrings();
                SetupSubmissionDatabaseName();
                SetupPeriodEndDatabaseName();
                SetupBracketedDatabaseNames();
                SetupAsseblyDirectory();
            }
            catch (Exception ex)
            {
                throw new GlobalTestContextSetupException(ex);
            }
        }

        public string SubmissionConnectionString { get; private set; }
        public string PeriodEndConnectionString { get; private set; }
        public string SubmissionDatabaseName { get; private set; }
        public string PeriodEndDatabaseName { get; private set; }
        public string BracketedSubmissionDatabaseName { get; private set; }
        public string BracketedPeriodEndDatabaseName { get; private set; }
        public string AssemblyDirectory { get; private set; }

        private void SetupConnectionStrings()
        {
            SubmissionConnectionString = Environment.GetEnvironmentVariable(SubmissionConnectionStringKey);
            if (string.IsNullOrEmpty(SubmissionConnectionString))
            {
                SubmissionConnectionString = ConfigurationManager.AppSettings[SubmissionConnectionStringKey];
            }

            PeriodEndConnectionString = Environment.GetEnvironmentVariable(PeriodEndConnectionStringKey);
            if (string.IsNullOrEmpty(PeriodEndConnectionString))
            {
                PeriodEndConnectionString = ConfigurationManager.AppSettings[PeriodEndConnectionStringKey];
            }
        }

        private void SetupSubmissionDatabaseName()
        {
            var match = Regex.Match(SubmissionConnectionString, @"database=([A-Z0-9\-_]{1,});", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                SubmissionDatabaseName = match.Groups[1].Value;
                return;
            }

            match = Regex.Match(SubmissionConnectionString, @"initial catalog=([A-Z0-9\-_]{1,});", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                SubmissionDatabaseName = match.Groups[1].Value;
                return;
            }

            throw new Exception("Cannot extract ilr submission database name from connection");
        }

        private void SetupPeriodEndDatabaseName()
        {
            var match = Regex.Match(PeriodEndConnectionString, @"database=([A-Z0-9\-_]{1,});", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                PeriodEndDatabaseName = match.Groups[1].Value;
                return;
            }

            match = Regex.Match(PeriodEndConnectionString, @"initial catalog=([A-Z0-9\-_]{1,});", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                PeriodEndDatabaseName = match.Groups[1].Value;
                return;
            }

            throw new Exception("Cannot extract period end database name from connection");
        }

        private void SetupBracketedDatabaseNames()
        {
            BracketedSubmissionDatabaseName = $"[{SubmissionDatabaseName}]";
            BracketedPeriodEndDatabaseName = $"[{PeriodEndDatabaseName}]";
        }

        private void SetupAsseblyDirectory()
        {
            AssemblyDirectory = System.IO.Path.GetDirectoryName(typeof(GlobalTestContext).Assembly.Location);
        }

        private static GlobalTestContext _instance;

        public static GlobalTestContext Instance
        {
            get { return _instance ?? (_instance = new GlobalTestContext()); }
        }
    }
}
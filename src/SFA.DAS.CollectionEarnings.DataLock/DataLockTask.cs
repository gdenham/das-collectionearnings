using NLog;
using SFA.DAS.CollectionEarnings.Contract;
using SFA.DAS.CollectionEarnings.Domain.DependencyResolution;
using SFA.DAS.CollectionEarnings.Infrastructure.DependencyResolution;
using SFA.DAS.CollectionEarnings.Infrastructure.Logging;
using System;
using System.Collections.Generic;

namespace SFA.DAS.CollectionEarnings.DataLock
{
    public class DataLockTask : IExternalTask
    {
        private readonly IDependencyResolver _dependencyResolver;
        private string transientConnectionString;
        private string logLevel;

        public DataLockTask()
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        internal DataLockTask(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public void Execute(IDictionary<string, string> context)
        {
            ValidateContext(context);

            _dependencyResolver.Init(typeof(DataLockProcessor));

            transientConnectionString = context[DasContextPropertyKeys.TransientDatabaseConnectionString];
            logLevel = context[DasContextPropertyKeys.LogLevel];

            LoggingConfig.ConfigureLogging(transientConnectionString, logLevel);

            var logger = _dependencyResolver.GetInstance<ILogger>();

            var processor = new DataLockProcessor(logger);

            processor.Process();
        }

        private void ValidateContext(IDictionary<string, string> context)
        {
            if (!context.ContainsKey(DasContextPropertyKeys.TransientDatabaseConnectionString))
            {
                throw new ArgumentNullException(DasContextPropertyKeys.TransientDatabaseConnectionString);
            }

            if (!context.ContainsKey(DasContextPropertyKeys.LogLevel))
            {
                throw new ArgumentNullException(DasContextPropertyKeys.LogLevel);
            }
        }
    }
}

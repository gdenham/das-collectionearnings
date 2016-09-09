using System;
using CS.Common.External.Interfaces;
using NLog;
using SFA.DAS.CollectionEarnings.DataLock.Context;
using SFA.DAS.CollectionEarnings.DataLock.DependencyResolution;
using SFA.DAS.CollectionEarnings.DataLock.Exceptions;
using SFA.DAS.CollectionEarnings.DataLock.Logging;

namespace SFA.DAS.CollectionEarnings.DataLock
{
    public class DataLockTask : IExternalTask
    {
        private readonly IDependencyResolver _dependencyResolver;
        private ILogger _logger;

        public DataLockTask()
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        internal DataLockTask(IDependencyResolver dependencyResolver, ILogger logger)
        {
            _dependencyResolver = dependencyResolver;
            _logger = logger;
        }

        public void Execute(IExternalContext context)
        {
            var contextWrapper = new ContextWrapper(context);

            if (IsValidContext(contextWrapper))
            {

                LoggingConfig.ConfigureLogging(
                    contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString),
                    contextWrapper.GetPropertyValue(ContextPropertyKeys.LogLevel)
                    );

                _dependencyResolver.Init(
                    typeof (DataLockProcessor),
                    contextWrapper
                    );

                if (_logger == null)
                {
                    _logger = LogManager.GetCurrentClassLogger();
                }

                try
                {
                    var processor = _dependencyResolver.GetInstance<DataLockProcessor>();

                    processor.Process();
                }
                catch (Exception ex)
                {
                    _logger.Fatal(ex, ex.Message);
                    throw;
                }
            }
        }

        private static bool IsValidContext(ContextWrapper contextWrapper)
        {
            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString)))
            {
                throw new DataLockInvalidContextException(DataLockExceptionMessages.ContextPropertiesNoConnectionString);
            }

            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.LogLevel)))
            {
                throw new DataLockInvalidContextException(DataLockExceptionMessages.ContextPropertiesNoLogLevel);
            }

            return true;
        }
    }
}

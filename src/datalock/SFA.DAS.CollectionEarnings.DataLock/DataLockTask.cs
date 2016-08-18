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

        public DataLockTask()
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        internal DataLockTask(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public void Execute(IExternalContext context)
        {
            _dependencyResolver.Init(typeof(DataLockProcessor));
            var contextWrapper = new ContextWrapper(context);

            ValidateContext(contextWrapper);

            LoggingConfig.ConfigureLogging(
                contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString),
                contextWrapper.GetPropertyValue(ContextPropertyKeys.LogLevel)
            );

            var logger = _dependencyResolver.GetInstance<ILogger>();

            var processor = new DataLockProcessor(logger);

            processor.Process();
        }

        private static void ValidateContext(ContextWrapper contextWrapper)
        {
            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString)))
            {
                throw new DataLockInvalidContextException(DataLockExceptionMessages.ContextPropertiesNoConnectionString);
            }

            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.LogLevel)))
            {
                throw new DataLockInvalidContextException(DataLockExceptionMessages.ContextPropertiesNoLogLevel);
            }
        }
    }
}

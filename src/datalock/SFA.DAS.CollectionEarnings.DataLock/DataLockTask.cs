using CS.Common.External.Interfaces;
using NLog;
using SFA.DAS.CollectionEarnings.Domain.DependencyResolution;
using SFA.DAS.CollectionEarnings.Infrastructure.DependencyResolution;
using SFA.DAS.CollectionEarnings.Infrastructure.Logging;
using SFA.DAS.CollectionEarnings.Infrastructure.Context;
using SFA.DAS.CollectionEarnings.Infrastructure.Exceptions;

namespace SFA.DAS.CollectionEarnings.DataLock
{
    public class DataLockTask : IExternalTask
    {
        private readonly IDependencyResolver _dependencyResolver;
        private ContextWrapper _contextWrapper;

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
            _contextWrapper = new ContextWrapper(context);

            ValidateContext(_contextWrapper);

            LoggingConfig.ConfigureLogging(
                _contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString),
                _contextWrapper.GetPropertyValue(ContextPropertyKeys.LogLevel)
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

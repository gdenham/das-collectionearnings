using System;
using CS.Common.External.Interfaces;
using MediatR;
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
            var contextWrapper = new ContextWrapper(context);

            ValidateContext(contextWrapper);

            LoggingConfig.ConfigureLogging(
                contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString),
                contextWrapper.GetPropertyValue(ContextPropertyKeys.LogLevel)
            );

            _dependencyResolver.Init(
                typeof(DataLockProcessor),
                contextWrapper
            );

            var logger = _dependencyResolver.GetInstance<ILogger>();
            var mediator = _dependencyResolver.GetInstance<IMediator>();

            try
            {
                var processor = new DataLockProcessor(logger, mediator);

                processor.Process();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw;
            }
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

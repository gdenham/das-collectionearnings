using CS.Common.External.Interfaces;
using NLog;
using SFA.DAS.CollectionEarnings.Domain.DependencyResolution;
using SFA.DAS.CollectionEarnings.Infrastructure.DcContext;
using SFA.DAS.CollectionEarnings.Infrastructure.DependencyResolution;
using SFA.DAS.CollectionEarnings.Infrastructure.Logging;
using System;

namespace SFA.DAS.CollectionEarnings.DataLock
{
    public class DataLockTask : IExternalTask
    {
        private readonly IDependencyResolver _dependencyResolver;
        private DcContextWrapper _contextWrapper;

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
            _contextWrapper = new DcContextWrapper(context);

            ValidateContext(_contextWrapper);

            LoggingConfig.ConfigureLogging(
                _contextWrapper.GetPropertyValue(DcContextPropertyKeys.TransientDatabaseConnectionString),
                _contextWrapper.GetPropertyValue(DcContextPropertyKeys.LogLevel)
            );

            var logger = _dependencyResolver.GetInstance<ILogger>();

            var processor = new DataLockProcessor(logger);

            processor.Process();
        }

        private void ValidateContext(DcContextWrapper contextWrapper)
        {
            if (contextWrapper.GetPropertyValue(DcContextPropertyKeys.TransientDatabaseConnectionString) == null)
            {
                throw new ArgumentNullException(DcContextPropertyKeys.TransientDatabaseConnectionString);
            }

            if (contextWrapper.GetPropertyValue(DcContextPropertyKeys.LogLevel) == null)
            {
                throw new ArgumentNullException(DcContextPropertyKeys.LogLevel);
            }
        }
    }
}

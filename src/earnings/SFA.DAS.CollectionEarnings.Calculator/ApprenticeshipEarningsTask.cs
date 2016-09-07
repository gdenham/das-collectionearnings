using System;
using CS.Common.External.Interfaces;
using NLog;
using SFA.DAS.CollectionEarnings.Calculator.Context;
using SFA.DAS.CollectionEarnings.Calculator.DependencyResolution;
using SFA.DAS.CollectionEarnings.Calculator.Exceptions;
using SFA.DAS.CollectionEarnings.Calculator.Logging;

namespace SFA.DAS.CollectionEarnings.Calculator
{
    public class ApprenticeshipEarningsTask : IExternalTask
    {
        private readonly IDependencyResolver _dependencyResolver;
        private ILogger _logger;

        public ApprenticeshipEarningsTask()
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        internal ApprenticeshipEarningsTask(IDependencyResolver dependencyResolver, ILogger logger)
        {
            _dependencyResolver = dependencyResolver;
            _logger = logger;
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
                typeof(ApprenticeshipEarningsProcessor),
                contextWrapper
            );

            if (_logger == null)
            {
                _logger = LogManager.GetCurrentClassLogger();
            }

            try
            {
                var processor = _dependencyResolver.GetInstance<ApprenticeshipEarningsProcessor>();

                processor.Process();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, ex.Message);
                throw;
            }
        }

        private void ValidateContext(ContextWrapper contextWrapper)
        {
            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString)))
            {
                throw new EarningsCalculatorInvalidContextException(EarningsCalculatorExceptionMessages.ContextPropertiesNoConnectionString);
            }

            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.LogLevel)))
            {
                throw new EarningsCalculatorInvalidContextException(EarningsCalculatorExceptionMessages.ContextPropertiesNoLogLevel);
            }
        }
    }
}
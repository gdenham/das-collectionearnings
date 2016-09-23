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

            if (IsContextValid(contextWrapper))
            {

                LoggingConfig.ConfigureLogging(
                    contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString),
                    contextWrapper.GetPropertyValue(ContextPropertyKeys.LogLevel)
                    );

                _dependencyResolver.Init(
                    typeof (ApprenticeshipEarningsPassThroughProcessor),
                    contextWrapper
                    );

                if (_logger == null)
                {
                    _logger = LogManager.GetCurrentClassLogger();
                }

                try
                {
                    var processor = _dependencyResolver.GetInstance<ApprenticeshipEarningsPassThroughProcessor>();

                    processor.Process();
                }
                catch (Exception ex)
                {
                    _logger.Fatal(ex, ex.Message);
                    throw;
                }
            }
        }

        private bool IsContextValid(ContextWrapper contextWrapper)
        {
            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString)))
            {
                throw new EarningsCalculatorInvalidContextException(EarningsCalculatorExceptionMessages.ContextPropertiesNoConnectionString);
            }

            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.LogLevel)))
            {
                throw new EarningsCalculatorInvalidContextException(EarningsCalculatorExceptionMessages.ContextPropertiesNoLogLevel);
            }

            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.YearOfCollection)))
            {
                throw new EarningsCalculatorInvalidContextException(EarningsCalculatorExceptionMessages.ContextPropertiesNoYearOfCollection);
            }

            return ValidateYearOfCollection(contextWrapper.GetPropertyValue(ContextPropertyKeys.YearOfCollection));
        }

        private bool ValidateYearOfCollection(string yearOfCollection)
        {
            int year1;
            int year2;

            if (yearOfCollection.Length != 4 ||
                !int.TryParse(yearOfCollection.Substring(0, 2), out year1) ||
                !int.TryParse(yearOfCollection.Substring(2, 2), out year2) ||
                (year2 != year1 + 1))
            {
                throw new EarningsCalculatorInvalidContextException(EarningsCalculatorExceptionMessages.ContextPropertiesInvalidYearOfCollection);
            }

            return true;
        }
    }
}
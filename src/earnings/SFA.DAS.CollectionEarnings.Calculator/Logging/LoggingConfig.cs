﻿using System;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using SFA.DAS.CollectionEarnings.Calculator.Exceptions;

namespace SFA.DAS.CollectionEarnings.Calculator.Logging
{
    public class LoggingConfig
    {
        public static void ConfigureLogging(string connectionString, string logLevel)
        {
            var config = new LoggingConfiguration();
            var sqlServerTarget = new DatabaseTarget("sqlserver");

            sqlServerTarget.ConnectionString = connectionString;

            sqlServerTarget.CommandText = @"INSERT INTO [Earnings].[TaskLog] (
                                        Level, Logger, Message, Exception
                                    ) VALUES (
                                        @level, @logger, @message, @exception
                                    )";

            sqlServerTarget.Parameters.Add(new DatabaseParameterInfo("@level", new SimpleLayout("${level}")));
            sqlServerTarget.Parameters.Add(new DatabaseParameterInfo("@logger", new SimpleLayout("${logger}")));
            sqlServerTarget.Parameters.Add(new DatabaseParameterInfo("@message", new SimpleLayout("${message}")));
            sqlServerTarget.Parameters.Add(new DatabaseParameterInfo("@exception", new SimpleLayout("${exception:tostring}")));

            config.AddTarget("sqlserver", sqlServerTarget);
            config.LoggingRules.Add(new LoggingRule("*", GetLogLevel(logLevel), sqlServerTarget));

            LogManager.Configuration = config;
            LogManager.ThrowExceptions = true;
        }

        private static LogLevel GetLogLevel(string logLevel)
        {
            try
            {
                return LogLevel.FromString(logLevel);
            }
            catch (Exception ex)
            {
                throw new EarningsCalculatorInvalidContextException(EarningsCalculatorExceptionMessages.ContextPropertiesInvalidLogLevel, ex);
            }
        }
    }
}

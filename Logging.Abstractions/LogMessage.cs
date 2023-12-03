using Microsoft.Extensions.Logging;

namespace Logging.Abstractions;

public static class LogMessage
{
    public static Action<ILogger, Exception?> Define(
        LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options)
    {
        var formatter = CreateLogValuesFormatter(formatString, expectedParameters: 0);

        void Log(ILogger logger, Exception? exception)
        {
            var logValues = new LogValues(formatter);
            var callback = LogValues.Callback;
            logger.Log(logLevel, eventId, logValues, exception, callback);
        }

        if (options is { SkipEnabledCheck: true })
        {
            return Log;
        }

        return (logger, exception) =>
        {
            if (logger.IsEnabled(logLevel))
            {
                Log(logger, exception);
            }
        };
    }

    public static Action<ILogger, T1, Exception?> Define<T1>(
        LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options)
    {
        var formatter = CreateLogValuesFormatter(formatString, expectedParameters: 1);

        void Log(ILogger logger, T1 arg1, Exception? exception)
        {
            var logValues = new LogValues<T1>(formatter, arg1);
            var callback = LogValues<T1>.Callback;
            logger.Log(logLevel, eventId, logValues, exception, callback);
        }

        if (options is { SkipEnabledCheck: true })
        {
            return Log;
        }

        return (logger, arg1, exception) =>
        {
            if (logger.IsEnabled(logLevel))
            {
                Log(logger, arg1, exception);
            }
        };
    }

    public static Action<ILogger, T1, T2, Exception?> Define<T1, T2>(
        LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options)
    {
        var formatter = CreateLogValuesFormatter(formatString, expectedParameters: 2);

        void Log(ILogger logger, T1 arg1, T2 arg2, Exception? exception)
        {
            var logValues = new LogValues<T1, T2>(formatter, arg1, arg2);
            var callback = LogValues<T1, T2>.Callback;
            logger.Log(logLevel, eventId, logValues, exception, callback);
        }

        if (options is { SkipEnabledCheck: true })
        {
            return Log;
        }

        return (logger, arg1, arg2, exception) =>
        {
            if (logger.IsEnabled(logLevel))
            {
                Log(logger, arg1, arg2, exception);
            }
        };
    }

    public static Action<ILogger, T1, T2, T3, Exception?> Define<T1, T2, T3>(
        LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options)
    {
        var formatter = CreateLogValuesFormatter(formatString, expectedParameters: 3);

        void Log(ILogger logger, T1 arg1, T2 arg2, T3 arg3, Exception? exception)
        {
            var logValues = new LogValues<T1, T2, T3>(formatter, arg1, arg2, arg3);
            var callback = LogValues<T1, T2, T3>.Callback;
            logger.Log(logLevel, eventId, logValues, exception, callback);
        }

        if (options is { SkipEnabledCheck: true })
        {
            return Log;
        }

        return (logger, arg1, arg2, arg3, exception) =>
        {
            if (logger.IsEnabled(logLevel))
            {
                Log(logger, arg1, arg2, arg3, exception);
            }
        };
    }

    public static Action<ILogger, T1, T2, T3, T4, Exception?> Define<T1, T2, T3, T4>(
        LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options)
    {
        var formatter = CreateLogValuesFormatter(formatString, expectedParameters: 4);

        void Log(ILogger logger, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Exception? exception)
        {
            var logValues = new LogValues<T1, T2, T3, T4>(formatter, arg1, arg2, arg3, arg4);
            var callback = LogValues<T1, T2, T3, T4>.Callback;
            logger.Log(logLevel, eventId, logValues, exception, callback);
        }

        if (options is { SkipEnabledCheck: true })
        {
            return Log;
        }

        return (logger, arg1, arg2, arg3, arg4, exception) =>
        {
            if (logger.IsEnabled(logLevel))
            {
                Log(logger, arg1, arg2, arg3, arg4, exception);
            }
        };
    }

    public static Action<ILogger, T1, T2, T3, T4, T5, Exception?> Define<T1, T2, T3, T4, T5>(
        LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options)
    {
        var formatter = CreateLogValuesFormatter(formatString, expectedParameters: 5);

        void Log(ILogger logger, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Exception? exception)
        {
            var logValues = new LogValues<T1, T2, T3, T4, T5>(formatter, arg1, arg2, arg3, arg4, arg5);
            var callback = LogValues<T1, T2, T3, T4, T5>.Callback;
            logger.Log(logLevel, eventId, logValues, exception, callback);
        }

        if (options is { SkipEnabledCheck: true })
        {
            return Log;
        }

        return (logger, arg1, arg2, arg3, arg4, arg5, exception) =>
        {
            if (logger.IsEnabled(logLevel))
            {
                Log(logger, arg1, arg2, arg3, arg4, arg5, exception);
            }
        };
    }

    public static Action<ILogger, T1, T2, T3, T4, T5, T6, Exception?> Define<T1, T2, T3, T4, T5, T6>(
        LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options)
    {
        var formatter = CreateLogValuesFormatter(formatString, expectedParameters: 6);

        void Log(ILogger logger, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, Exception? exception)
        {
            var logValues = new LogValues<T1, T2, T3, T4, T5, T6>(formatter, arg1, arg2, arg3, arg4, arg5, arg6);
            var callback = LogValues<T1, T2, T3, T4, T5, T6>.Callback;
            logger.Log(logLevel, eventId, logValues, exception, callback);
        }

        if (options is { SkipEnabledCheck: true })
        {
            return Log;
        }

        return (logger, arg1, arg2, arg3, arg4, arg5, arg6, exception) =>
        {
            if (logger.IsEnabled(logLevel))
            {
                Log(logger, arg1, arg2, arg3, arg4, arg5, arg6, exception);
            }
        };
    }

    private static LogValuesFormatter CreateLogValuesFormatter(
        string formatString, int expectedParameters)
    {
        var logValuesFormatter = new LogValuesFormatter(formatString);

        var actualCount = logValuesFormatter.ValueNames.Count;
        if (actualCount != expectedParameters)
        {
            throw new ArgumentException($"Failed to parse: {formatString}");
        }

        return logValuesFormatter;
    }
}
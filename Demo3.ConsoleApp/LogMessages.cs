using Logging.Abstractions;

using Microsoft.Extensions.Logging;

namespace Demo3.ConsoleApp;

public static partial class LogMessages
{
    [LogMessage(1, LogLevel.Information, "Message1 {Number}")]
    public static partial void LogMessage1(this ILogger logger, int number);

    [LogMessage(2, LogLevel.Information, "Message2 {Number}")]
    public static partial void LogMessage2(this ILogger logger, int number);
}
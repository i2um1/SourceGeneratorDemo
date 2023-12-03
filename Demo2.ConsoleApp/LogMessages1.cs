using Logging.Abstractions;

using Microsoft.Extensions.Logging;

namespace Demo2.ConsoleApp;

public static partial class LogMessages1
{
    [LogMessage(1, LogLevel.Information, "Message1 {Number}")]
    public static partial void LogMessage1(this ILogger logger, int number);
}
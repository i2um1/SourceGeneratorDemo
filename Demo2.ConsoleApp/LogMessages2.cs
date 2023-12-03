using Logging.Abstractions;

using Microsoft.Extensions.Logging;

namespace Demo2.ConsoleApp;

public static partial class LogMessages2
{
    [LogMessage(2, LogLevel.Information, "Message2 {Number}")]
    public static partial void LogMessage2(this ILogger logger, int number);
}
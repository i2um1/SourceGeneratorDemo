using Logging.Abstractions;

using Microsoft.Extensions.Logging;

namespace Demo3.ConsoleApp;

public static partial class OtherLogMessages
{
    [LogMessage(3, LogLevel.Information, "Message3 {Number}")]
    public static partial void LogMessage3(this ILogger logger, int number);
}
using Logging.Abstractions;

using Microsoft.Extensions.Logging;

namespace Demo1.ConsoleApp;

public static partial class LogMessages
{
    [LogMessage(1, LogLevel.Information, "Job {JobId} ({JobName}) started")]
    public static partial void LogJobStarted(this ILogger logger, int jobId, string jobName);
}
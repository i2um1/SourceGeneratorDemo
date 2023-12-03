using Microsoft.Extensions.Logging;

namespace Logging.Abstractions;

[AttributeUsage(AttributeTargets.Method)]
public sealed class LogMessageAttribute : Attribute
{
    public LogMessageAttribute(int eventId, LogLevel level, string message)
    {
        EventId = eventId;
        Level = level;
        Message = message;
    }

    public LogLevel Level { get; set; }

    public int EventId { get; set; }

    public string? EventName { get; set; }

    public string Message { get; set; }

    public bool SkipEnabledCheck { get; set; }
}
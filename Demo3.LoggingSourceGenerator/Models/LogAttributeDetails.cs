namespace Demo3.LoggingSourceGenerator.Models;

internal sealed class LogAttributeDetails
{
    public int Level { get; set; }
    public int EventId { get; set; }
    public string? EventName { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool SkipEnabledCheck { get; set; }
}
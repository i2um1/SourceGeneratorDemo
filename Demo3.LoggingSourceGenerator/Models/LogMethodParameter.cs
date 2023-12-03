namespace Demo3.LoggingSourceGenerator.Models;

public readonly struct LogMethodParameter
{
    public LogMethodParameter(string name, string type)
    {
        Name = name;
        Type = type;
        IsLogger = type is "global::Microsoft.Extensions.Logging.ILogger";
        IsException = type is "global::System.Exception";
    }

    public string Name { get; }
    public string Type { get; }
    public bool IsLogger { get; }
    public bool IsException { get; }
}
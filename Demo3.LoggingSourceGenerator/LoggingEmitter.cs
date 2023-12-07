using System.Text;

using Demo3.LoggingSourceGenerator.Models;

using Microsoft.CodeAnalysis;

namespace Demo3.LoggingSourceGenerator;

internal static class LoggingEmitter
{
    private static readonly string[] _logLevels =
    {
        "Trace", "Debug", "Information", "Warning", "Error", "Critical", "None"
    };

    public static void EmitMethod(
        StringBuilder stringBuilder,
        IMethodSymbol methodSymbol,
        IReadOnlyList<LogMethodParameter> methodParameters)
    {
        stringBuilder.Append($"    public static partial void {methodSymbol.Name}(");
        EmitMethodParameters(stringBuilder, methodParameters);
        stringBuilder.AppendLine(")");
        stringBuilder.AppendLine("    {");
        EmitActionInvocation(stringBuilder, methodSymbol, methodParameters);
        stringBuilder.AppendLine("    }");
    }

    public static void EmitLogAction(
        StringBuilder stringBuilder,
        IMethodSymbol methodSymbol,
        IReadOnlyList<LogMethodParameter> methodParameters,
        LogAttributeDetails details)
    {
        stringBuilder.Append("    private static readonly Action<");
        for (var i = 0; i < methodParameters.Count; i++)
        {
            if (methodParameters[i].IsException)
            {
                continue;
            }

            if (i > 0)
            {
                stringBuilder.Append(", ");
            }

            stringBuilder.Append($"{methodParameters[i].Type}");
        }

        stringBuilder.AppendLine($", global::System.Exception?> _cached{methodSymbol.Name}Action =");
        stringBuilder.Append("        global::Logging.Abstractions.LogMessage.Define<");
        for (var i = 1; i < methodParameters.Count; i++)
        {
            if (methodParameters[i].IsException)
            {
                continue;
            }

            if (i > 1 && !(i is 2 && methodParameters[1].IsException))
            {
                stringBuilder.Append(", ");
            }

            stringBuilder.Append($"{methodParameters[i].Type}");
        }

        stringBuilder.Append(">(");

        const string LOGGING_NAMESPACE = "global::Microsoft.Extensions.Logging";
        stringBuilder.Append($"{LOGGING_NAMESPACE}.LogLevel.{_logLevels[details.Level]}, ");
        var eventName = details.EventName is null ? null : $", \"{details.EventName}\"";
        stringBuilder.Append($"new {LOGGING_NAMESPACE}.EventId({details.EventId}{eventName}), ");
        stringBuilder.Append($"\"{details.Message}\"");
        if (details.SkipEnabledCheck)
        {
            stringBuilder.Append($", new {LOGGING_NAMESPACE}.LogDefineOptions {{ SkipEnabledCheck = true }}");
        }
        else
        {
            stringBuilder.Append(", null");
        }

        stringBuilder.AppendLine(");");
        stringBuilder.AppendLine();
    }

    private static void EmitActionInvocation(
        StringBuilder stringBuilder,
        IMethodSymbol methodSymbol,
        IReadOnlyList<LogMethodParameter> methodParameters)
    {
        var exceptionParameterName = "null";
        foreach (var methodParameter in methodParameters)
        {
            if (methodParameter.IsException)
            {
                exceptionParameterName = methodParameter.Name;
            }
        }

        stringBuilder.Append($"        _cached{methodSymbol.Name}Action(");
        for (var i = 0; i < methodParameters.Count; i++)
        {
            if (methodParameters[i].IsException)
            {
                continue;
            }

            if (i > 0)
            {
                stringBuilder.Append(", ");
            }

            stringBuilder.Append(methodParameters[i].Name);
        }

        stringBuilder.AppendLine($", {exceptionParameterName});");
    }

    private static void EmitMethodParameters(
        StringBuilder stringBuilder, IReadOnlyList<LogMethodParameter> logMethodParameters)
    {
        for (var i = 0; i < logMethodParameters.Count; i++)
        {
            if (logMethodParameters[i].IsLogger)
            {
                stringBuilder.Append("this ");
            }

            if (i > 0)
            {
                stringBuilder.Append(", ");
            }

            stringBuilder.Append($"{logMethodParameters[i].Type} {logMethodParameters[i].Name}");
        }
    }
}
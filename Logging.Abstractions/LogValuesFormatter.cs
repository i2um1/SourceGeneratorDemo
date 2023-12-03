using System.Collections;
using System.Globalization;

namespace Logging.Abstractions;

internal sealed class LogValuesFormatter
{
    private const string NULL_VALUE = "(null)";
    private static readonly char[] _formatDelimiters = { ',', ':' };
    private readonly string _format;
    private readonly List<string> _valueNames = new();

    public LogValuesFormatter(string format)
    {
        OriginalFormat = format;

        var vsb = new ValueStringBuilder(stackalloc char[256]);
        var scanIndex = 0;
        var endIndex = format.Length;

        while (scanIndex < endIndex)
        {
            var openBraceIndex = FindBraceIndex(format, '{', scanIndex, endIndex);
            if (scanIndex == 0 && openBraceIndex == endIndex)
            {
                _format = format;
                return;
            }

            var closeBraceIndex = FindBraceIndex(format, '}', openBraceIndex, endIndex);

            if (closeBraceIndex == endIndex)
            {
                vsb.Append(format.AsSpan(scanIndex, endIndex - scanIndex));
                scanIndex = endIndex;
            }
            else
            {
                // Format item syntax : { index[,alignment][ :formatString] }.
                var formatDelimiterIndex = FindIndexOfAny(format, _formatDelimiters, openBraceIndex, closeBraceIndex);

                vsb.Append(format.AsSpan(scanIndex, openBraceIndex - scanIndex + 1));
                vsb.Append(_valueNames.Count.ToString());
                _valueNames.Add(format.Substring(openBraceIndex + 1, formatDelimiterIndex - openBraceIndex - 1));
                vsb.Append(format.AsSpan(formatDelimiterIndex, closeBraceIndex - formatDelimiterIndex + 1));

                scanIndex = closeBraceIndex + 1;
            }
        }

        _format = vsb.ToString();
    }

    public string OriginalFormat { get; }
    public List<string> ValueNames => _valueNames;

    private static int FindBraceIndex(string format, char brace, int startIndex, int endIndex)
    {
        // Example: {{prefix{{{Argument}}}suffix}}.
        var braceIndex = endIndex;
        var scanIndex = startIndex;
        var braceOccurrenceCount = 0;

        while (scanIndex < endIndex)
        {
            if (braceOccurrenceCount > 0 && format[scanIndex] != brace)
            {
                if (braceOccurrenceCount % 2 == 0)
                {
                    // Even number of '{' or '}' found. Proceed search with next occurrence of '{' or '}'.
                    braceOccurrenceCount = 0;
                    braceIndex = endIndex;
                }
                else
                {
                    // An unescaped '{' or '}' found.
                    break;
                }
            }
            else if (format[scanIndex] == brace)
            {
                if (brace == '}')
                {
                    if (braceOccurrenceCount == 0)
                    {
                        // For '}' pick the first occurrence.
                        braceIndex = scanIndex;
                    }
                }
                else
                {
                    // For '{' pick the last occurrence.
                    braceIndex = scanIndex;
                }

                braceOccurrenceCount++;
            }

            scanIndex++;
        }

        return braceIndex;
    }

    private static int FindIndexOfAny(string format, char[] chars, int startIndex, int endIndex)
    {
        var findIndex = format.IndexOfAny(chars, startIndex, endIndex - startIndex);
        return findIndex == -1 ? endIndex : findIndex;
    }

    internal string FormatWithOverwrite(object?[]? values)
    {
        if (values != null)
        {
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = FormatArgument(values[i]);
            }
        }

        return string.Format(CultureInfo.InvariantCulture, _format, values ?? Array.Empty<object>());
    }

    internal string Format()
    {
        return _format;
    }

    internal string Format(object? arg0)
    {
        return string.Format(CultureInfo.InvariantCulture, _format, FormatArgument(arg0));
    }

    internal string Format(object? arg0, object? arg1)
    {
        return string.Format(CultureInfo.InvariantCulture, _format, FormatArgument(arg0), FormatArgument(arg1));
    }

    internal string Format(object? arg0, object? arg1, object? arg2)
    {
        return string.Format(CultureInfo.InvariantCulture, _format, FormatArgument(arg0), FormatArgument(arg1),
            FormatArgument(arg2));
    }

    private static object FormatArgument(object? value)
    {
        if (value == null)
        {
            return NULL_VALUE;
        }

        if (value is string)
        {
            return value;
        }

        if (value is IEnumerable enumerable)
        {
            var vsb = new ValueStringBuilder(stackalloc char[256]);
            var first = true;
            foreach (var e in enumerable)
            {
                if (!first)
                {
                    vsb.Append(", ");
                }

                vsb.Append(e != null ? e.ToString() : NULL_VALUE);
                first = false;
            }

            return vsb.ToString();
        }

        return value;
    }
}
using System.Collections;

namespace Logging.Abstractions;

internal readonly struct LogValues : IReadOnlyList<KeyValuePair<string, object?>>
{
    public static readonly Func<LogValues, Exception?, string> Callback =
        (state, _) => state.ToString();

    private readonly LogValuesFormatter _formatter;

    public LogValues(LogValuesFormatter formatter)
    {
        _formatter = formatter;
    }

    public KeyValuePair<string, object?> this[int index]
    {
        get
        {
            if (index is 0)
            {
                return new KeyValuePair<string, object?>("{OriginalFormat}", _formatter.OriginalFormat);
            }

            throw new IndexOutOfRangeException(nameof(index));
        }
    }

    public int Count => 1;

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        yield return this[0];
    }

    public override string ToString() => _formatter.Format();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

internal readonly struct LogValues<T0> : IReadOnlyList<KeyValuePair<string, object?>>
{
    public static readonly Func<LogValues<T0>, Exception?, string> Callback =
        (state, _) => state.ToString();

    private readonly LogValuesFormatter _formatter;
    private readonly T0 _value0;

    public LogValues(LogValuesFormatter formatter, T0 value0)
    {
        _formatter = formatter;
        _value0 = value0;
    }

    public KeyValuePair<string, object?> this[int index]
        => index switch
        {
            0 => new KeyValuePair<string, object?>(_formatter.ValueNames[0], _value0),
            1 => new KeyValuePair<string, object?>("{OriginalFormat}", _formatter.OriginalFormat),
            _ => throw new IndexOutOfRangeException(nameof(index))
        };

    public int Count => 2;

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        for (var i = 0; i < Count; ++i)
        {
            yield return this[i];
        }
    }

    public override string ToString() => _formatter.Format(_value0);

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

internal readonly struct LogValues<T0, T1> : IReadOnlyList<KeyValuePair<string, object?>>
{
    public static readonly Func<LogValues<T0, T1>, Exception?, string> Callback =
        (state, _) => state.ToString();

    private readonly LogValuesFormatter _formatter;
    private readonly T0 _value0;
    private readonly T1 _value1;

    public LogValues(LogValuesFormatter formatter, T0 value0, T1 value1)
    {
        _formatter = formatter;
        _value0 = value0;
        _value1 = value1;
    }

    public KeyValuePair<string, object?> this[int index]
        => index switch
        {
            0 => new KeyValuePair<string, object?>(_formatter.ValueNames[0], _value0),
            1 => new KeyValuePair<string, object?>(_formatter.ValueNames[1], _value1),
            2 => new KeyValuePair<string, object?>("{OriginalFormat}", _formatter.OriginalFormat),
            _ => throw new IndexOutOfRangeException(nameof(index))
        };

    public int Count => 3;

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        for (var i = 0; i < Count; ++i)
        {
            yield return this[i];
        }
    }

    public override string ToString() => _formatter.Format(_value0, _value1);

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

internal readonly struct LogValues<T0, T1, T2> : IReadOnlyList<KeyValuePair<string, object?>>
{
    public static readonly Func<LogValues<T0, T1, T2>, Exception?, string> Callback =
        (state, _) => state.ToString();

    private readonly LogValuesFormatter _formatter;
    private readonly T0 _value0;
    private readonly T1 _value1;
    private readonly T2 _value2;

    public int Count => 4;

    public KeyValuePair<string, object?> this[int index]
        => index switch
        {
            0 => new KeyValuePair<string, object?>(_formatter.ValueNames[0], _value0),
            1 => new KeyValuePair<string, object?>(_formatter.ValueNames[1], _value1),
            2 => new KeyValuePair<string, object?>(_formatter.ValueNames[2], _value2),
            3 => new KeyValuePair<string, object?>("{OriginalFormat}", _formatter.OriginalFormat),
            _ => throw new IndexOutOfRangeException(nameof(index))
        };

    public LogValues(LogValuesFormatter formatter, T0 value0, T1 value1, T2 value2)
    {
        _formatter = formatter;
        _value0 = value0;
        _value1 = value1;
        _value2 = value2;
    }

    public override string ToString() => _formatter.Format(_value0, _value1, _value2);

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        for (var i = 0; i < Count; ++i)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

internal readonly struct LogValues<T0, T1, T2, T3> : IReadOnlyList<KeyValuePair<string, object?>>
{
    public static readonly Func<LogValues<T0, T1, T2, T3>, Exception?, string> Callback =
        (state, _) => state.ToString();

    private readonly LogValuesFormatter _formatter;
    private readonly T0 _value0;
    private readonly T1 _value1;
    private readonly T2 _value2;
    private readonly T3 _value3;

    public int Count => 5;

    public KeyValuePair<string, object?> this[int index]
        => index switch
        {
            0 => new KeyValuePair<string, object?>(_formatter.ValueNames[0], _value0),
            1 => new KeyValuePair<string, object?>(_formatter.ValueNames[1], _value1),
            2 => new KeyValuePair<string, object?>(_formatter.ValueNames[2], _value2),
            3 => new KeyValuePair<string, object?>(_formatter.ValueNames[3], _value3),
            4 => new KeyValuePair<string, object?>("{OriginalFormat}", _formatter.OriginalFormat),
            _ => throw new IndexOutOfRangeException(nameof(index))
        };

    public LogValues(LogValuesFormatter formatter, T0 value0, T1 value1, T2 value2, T3 value3)
    {
        _formatter = formatter;
        _value0 = value0;
        _value1 = value1;
        _value2 = value2;
        _value3 = value3;
    }

    private object?[] ToArray() => new object?[] { _value0, _value1, _value2, _value3 };

    public override string ToString() => _formatter.FormatWithOverwrite(ToArray());

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        for (var i = 0; i < Count; ++i)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

internal readonly struct LogValues<T0, T1, T2, T3, T4> : IReadOnlyList<KeyValuePair<string, object?>>
{
    public static readonly Func<LogValues<T0, T1, T2, T3, T4>, Exception?, string> Callback =
        (state, _) => state.ToString();

    private readonly LogValuesFormatter _formatter;
    private readonly T0 _value0;
    private readonly T1 _value1;
    private readonly T2 _value2;
    private readonly T3 _value3;
    private readonly T4 _value4;

    public int Count => 6;

    public KeyValuePair<string, object?> this[int index]
        => index switch
        {
            0 => new KeyValuePair<string, object?>(_formatter.ValueNames[0], _value0),
            1 => new KeyValuePair<string, object?>(_formatter.ValueNames[1], _value1),
            2 => new KeyValuePair<string, object?>(_formatter.ValueNames[2], _value2),
            3 => new KeyValuePair<string, object?>(_formatter.ValueNames[3], _value3),
            4 => new KeyValuePair<string, object?>(_formatter.ValueNames[4], _value4),
            5 => new KeyValuePair<string, object?>("{OriginalFormat}", _formatter.OriginalFormat),
            _ => throw new IndexOutOfRangeException(nameof(index))
        };

    public LogValues(LogValuesFormatter formatter, T0 value0, T1 value1, T2 value2, T3 value3, T4 value4)
    {
        _formatter = formatter;
        _value0 = value0;
        _value1 = value1;
        _value2 = value2;
        _value3 = value3;
        _value4 = value4;
    }

    private object?[] ToArray() => new object?[] { _value0, _value1, _value2, _value3, _value4 };

    public override string ToString() => _formatter.FormatWithOverwrite(ToArray());

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        for (var i = 0; i < Count; ++i)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

internal readonly struct LogValues<T0, T1, T2, T3, T4, T5> : IReadOnlyList<KeyValuePair<string, object?>>
{
    public static readonly Func<LogValues<T0, T1, T2, T3, T4, T5>, Exception?, string> Callback =
        (state, _) => state.ToString();

    private readonly LogValuesFormatter _formatter;
    private readonly T0 _value0;
    private readonly T1 _value1;
    private readonly T2 _value2;
    private readonly T3 _value3;
    private readonly T4 _value4;
    private readonly T5 _value5;

    public int Count => 7;

    public KeyValuePair<string, object?> this[int index]
        => index switch
        {
            0 => new KeyValuePair<string, object?>(_formatter.ValueNames[0], _value0),
            1 => new KeyValuePair<string, object?>(_formatter.ValueNames[1], _value1),
            2 => new KeyValuePair<string, object?>(_formatter.ValueNames[2], _value2),
            3 => new KeyValuePair<string, object?>(_formatter.ValueNames[3], _value3),
            4 => new KeyValuePair<string, object?>(_formatter.ValueNames[4], _value4),
            5 => new KeyValuePair<string, object?>(_formatter.ValueNames[5], _value5),
            6 => new KeyValuePair<string, object?>("{OriginalFormat}", _formatter.OriginalFormat),
            _ => throw new IndexOutOfRangeException(nameof(index))
        };

    public LogValues(LogValuesFormatter formatter, T0 value0, T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
    {
        _formatter = formatter;
        _value0 = value0;
        _value1 = value1;
        _value2 = value2;
        _value3 = value3;
        _value4 = value4;
        _value5 = value5;
    }

    private object?[] ToArray() => new object?[] { _value0, _value1, _value2, _value3, _value4, _value5 };

    public override string ToString() => _formatter.FormatWithOverwrite(ToArray());

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        for (var i = 0; i < Count; ++i)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
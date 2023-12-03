using System.Buffers;
using System.Runtime.CompilerServices;

namespace Logging.Abstractions;

internal ref struct ValueStringBuilder
{
    private char[]? _arrayToReturnToPool;
    private Span<char> _chars;

    public ValueStringBuilder(Span<char> initialBuffer)
    {
        _arrayToReturnToPool = null;
        _chars = initialBuffer;
        Length = 0;
    }

    public int Length { get; set; }

    public override string ToString()
    {
        var s = _chars.Slice(0, Length).ToString();
        Dispose();
        return s;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(char c)
    {
        var pos = Length;
        if ((uint)pos < (uint)_chars.Length)
        {
            _chars[pos] = c;
            Length = pos + 1;
        }
        else
        {
            GrowAndAppend(c);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(string? s)
    {
        if (s == null)
        {
            return;
        }

        var pos = Length;
        if (s.Length == 1 && (uint)pos < (uint)_chars.Length)
        {
            _chars[pos] = s[0];
            Length = pos + 1;
        }
        else
        {
            AppendSlow(s);
        }
    }

    private void AppendSlow(string s)
    {
        var pos = Length;
        if (pos > _chars.Length - s.Length)
        {
            Grow(s.Length);
        }

        s.CopyTo(_chars[pos..]);
        Length += s.Length;
    }

    public void Append(ReadOnlySpan<char> value)
    {
        var pos = Length;
        if (pos > _chars.Length - value.Length)
        {
            Grow(value.Length);
        }

        value.CopyTo(_chars.Slice(Length));
        Length += value.Length;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowAndAppend(char c)
    {
        Grow(1);
        Append(c);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void Grow(int additionalCapacityBeyondPos)
    {
        const uint ARRAY_MAX_LENGTH = 0x7FFFFFC7;

        var newCapacity = (int)Math.Max(
            (uint)(Length + additionalCapacityBeyondPos),
            Math.Min((uint)_chars.Length * 2, ARRAY_MAX_LENGTH));

        var poolArray = ArrayPool<char>.Shared.Rent(newCapacity);

        _chars[..Length].CopyTo(poolArray);

        var toReturn = _arrayToReturnToPool;
        _chars = _arrayToReturnToPool = poolArray;
        if (toReturn != null)
        {
            ArrayPool<char>.Shared.Return(toReturn);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        var toReturn = _arrayToReturnToPool;
        this = default;
        if (toReturn != null)
        {
            ArrayPool<char>.Shared.Return(toReturn);
        }
    }
}
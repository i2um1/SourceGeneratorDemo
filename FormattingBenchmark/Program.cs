using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<FormattingBenchmark>();

[MemoryDiagnoser]
public class FormattingBenchmark
{
    private string? _text;
    private string? _text2;
    private string? _text3;
    private int _parameter;
    private string? _parameterText;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _text = "text";
        _text2 = "text2";
        _text3 = "text3";
        _parameter = 5;
        _parameterText = "5";
    }

    [Benchmark]
    public string StringFormat()
    {
        return string.Format("Text = {0}, Text2 = {1}, Text3 = {2}, Parameter = {3}",
            _text, _text2, _text3, _parameter);
    }

    [Benchmark]
    public string StringFormatNoBox()
    {
        return string.Format("Text = {0}, Text2 = {1}, Text3 = {2}, Parameter = {3}",
            _text, _text2, _text3, _parameterText);
    }

    [Benchmark]
    public string StringConcat()
    {
        return string.Concat("Text = ", _text, ", Text2 = ", _text2,
            ", Text3 = ", _text3, ", Parameter = ", _parameter);
    }

    [Benchmark]
    public string InterpolatedString()
    {
        return $"Text = {_text}, Text2 = {_text2}, Text3 = {_text3}, Parameter = {_parameter}";
    }
}
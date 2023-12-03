using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Demo3.LoggingSourceGenerator.Models;

public sealed class MyCustomObject : IEquatable<MyCustomObject>
{
    public MyCustomObject(
        ImmutableArray<ClassDeclarationSyntax> classes,
        Compilation compilation)
    {
        Classes = classes.Distinct().ToImmutableArray();
        Compilation = compilation;
    }

    public ImmutableArray<ClassDeclarationSyntax> Classes { get; }

    public Compilation Compilation { get; }

    public bool Equals(MyCustomObject? other)
    {
        if (other is null || Classes.Length != other.Classes.Length)
        {
            return false;
        }

        for (var i = 0; i < Classes.Length; i++)
        {
            if (!Classes[i].Equals(other.Classes[i]))
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object? other)
    {
        return Equals(other as MyCustomObject);
    }

    public override int GetHashCode() => Classes.GetHashCode();
}
using System.Collections.Immutable;
using System.Text;

using Demo3.LoggingSourceGenerator.Models;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Demo3.LoggingSourceGenerator;

[Generator]
public sealed class LoggingGenerator : IIncrementalGenerator
{
    private static int _counter;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var methodDeclarations = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "Logging.Abstractions.LogMessageAttribute",
                static (syntaxNode, _) => syntaxNode is MethodDeclarationSyntax,
                static (syntaxContext, _) => syntaxContext.TargetNode.Parent as ClassDeclarationSyntax)
            .Where(static classSyntax => classSyntax is not null)
            .Collect();

        var methodDeclarationsAndCompilation = methodDeclarations
            .Combine(context.CompilationProvider)
            .Select((tuple, _) => new MyCustomObject(tuple.Left!, tuple.Right));

        context.RegisterSourceOutput(
            methodDeclarationsAndCompilation,
            static (sourceContext, customObject)
                => Execute(sourceContext, customObject.Compilation, customObject.Classes));
    }

    private static void Execute(
        SourceProductionContext sourceContext,
        Compilation compilation,
        ImmutableArray<ClassDeclarationSyntax> classes)
    {
        var logMessageAttribute = compilation.GetTypeByMetadataName("Logging.Abstractions.LogMessageAttribute");
        if (logMessageAttribute is null)
        {
            return;
        }

        foreach (var group in classes.GroupBy(x => x.SyntaxTree))
        {
            SemanticModel? semanticModel = null;
            var classFound = false;
            var result = new StringBuilder();

            foreach (var classSyntax in group.Distinct())
            {
                sourceContext.CancellationToken.ThrowIfCancellationRequested();

                foreach (var member in classSyntax.Members)
                {
                    if (member is not MethodDeclarationSyntax methodSyntax)
                    {
                        continue;
                    }

                    semanticModel ??= compilation.GetSemanticModel(classSyntax.SyntaxTree);
                    if (semanticModel.GetDeclaredSymbol(methodSyntax, sourceContext.CancellationToken)
                        is not IMethodSymbol logMethodSymbol)
                    {
                        return;
                    }

                    if (!classFound)
                    {
                        var typeNamespace = logMethodSymbol.ContainingNamespace.IsGlobalNamespace
                            ? null
                            : $"{logMethodSymbol.ContainingNamespace}";

                        Interlocked.Increment(ref _counter);
                        result.AppendLine("#nullable enable");
                        result.AppendLine();
                        result.AppendLine($"// Counter {_counter}");
                        result.AppendLine();
                        result.AppendLine($"namespace {typeNamespace};");
                        result.AppendLine();
                        result.AppendLine($"public static partial class {classSyntax.Identifier.Text}");
                        result.AppendLine("{");
                    }

                    classFound = true;

                    var logAttributes = LoggingParser.ParseLogAttributes(
                        methodSyntax, logMethodSymbol, semanticModel, logMessageAttribute, sourceContext.CancellationToken);

                    foreach (var logAttribute in logAttributes)
                    {
                        var logMethodParameters = LoggingParser.ParseLogMethodParameters(logMethodSymbol);

                        LoggingEmitter.EmitLogAction(result, logMethodSymbol, logMethodParameters, logAttribute);
                        LoggingEmitter.EmitMethod(result, logMethodSymbol, logMethodParameters);
                    }
                }
            }

            if (!classFound)
            {
                continue;
            }

            result.AppendLine("}");

            sourceContext.AddSource(
                $"{group.First().Identifier.Text}.g.cs",
                SourceText.From(result.ToString(), Encoding.UTF8));
        }
    }
}
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Demo1.LoggingSourceGenerator;

[Generator]
public sealed class LoggingGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var methodDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                // predicate
                static (syntaxNode, _) =>
                    syntaxNode is AttributeSyntax attribute
                    && ExtractName(attribute.Name) is "LogMessage" or "LogMessageAttribute",
                // transform
                static (syntaxContext, _) =>
                    syntaxContext.Node.Parent?.Parent as MethodDeclarationSyntax)
            .Where(static methodSyntax => methodSyntax is not null);

        var methodDeclarationsAndCompilation = methodDeclarations.Combine(context.CompilationProvider);

        context.RegisterSourceOutput(
            methodDeclarationsAndCompilation,
            static (sourceContext, tuple) => Execute(sourceContext, tuple.Right, tuple.Left!));
    }

    private static string? ExtractName(NameSyntax? name)
        => name switch
        {
            SimpleNameSyntax syntax => syntax.Identifier.Text,
            QualifiedNameSyntax syntax => syntax.Right.Identifier.Text,
            _ => null
        };

    private static void Execute(
        SourceProductionContext sourceContext,
        Compilation compilation,
        MethodDeclarationSyntax methodSyntax)
    {
        var logMessageAttribute = compilation.GetTypeByMetadataName("Logging.Abstractions.LogMessageAttribute");
        if (logMessageAttribute == null)
        {
            return;
        }

        var semanticModel = compilation.GetSemanticModel(methodSyntax.SyntaxTree);
        if (semanticModel.GetDeclaredSymbol(methodSyntax, sourceContext.CancellationToken)
            is not IMethodSymbol logMethodSymbol)
        {
            return;
        }

        var classSyntax = (ClassDeclarationSyntax)methodSyntax.Parent!;
        var className = classSyntax.Identifier.Text;
        var typeNamespace = logMethodSymbol.ContainingNamespace.IsGlobalNamespace
            ? null
            : $"{logMethodSymbol.ContainingNamespace}";

        var logAttributes = LoggingParser.ParseLogAttributes(
            methodSyntax, logMethodSymbol, semanticModel, logMessageAttribute, sourceContext.CancellationToken);

        foreach (var logAttribute in logAttributes)
        {
            var logMethodParameters = LoggingParser.ParseLogMethodParameters(logMethodSymbol);

            var result = new StringBuilder();
            result.AppendLine("#nullable enable");
            result.AppendLine();
            result.AppendLine($"namespace {typeNamespace};");
            result.AppendLine();
            result.AppendLine($"public static partial class {className}");
            result.AppendLine("{");
            LoggingEmitter.EmitLogAction(result, logMethodSymbol, logMethodParameters, logAttribute);
            LoggingEmitter.EmitMethod(result, logMethodSymbol, logMethodParameters);
            result.AppendLine("}");

            sourceContext.AddSource(
                $"{className}.{logMethodSymbol.Name}.g.cs",
                SourceText.From(result.ToString(), Encoding.UTF8));
        }
    }
}
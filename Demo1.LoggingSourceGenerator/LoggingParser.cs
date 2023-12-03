using Demo1.LoggingSourceGenerator.Models;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Demo1.LoggingSourceGenerator;

internal static class LoggingParser
{
    public static IEnumerable<LogAttributeDetails> ParseLogAttributes(
        MethodDeclarationSyntax methodSyntax,
        IMethodSymbol methodSymbol,
        SemanticModel semanticModel,
        INamedTypeSymbol logMessageAttribute,
        CancellationToken cancellationToken)
    {
        foreach (var attributeSyntaxList in methodSyntax.AttributeLists)
        {
            foreach (var attributeSyntax in attributeSyntaxList.Attributes)
            {
                if (semanticModel.GetSymbolInfo(attributeSyntax, cancellationToken).Symbol
                        is not IMethodSymbol attributeMethodSymbol
                    || !logMessageAttribute.Equals(attributeMethodSymbol.ContainingType,
                        SymbolEqualityComparer.Default))
                {
                    continue;
                }

                var boundAttributes = methodSymbol.GetAttributes();
                if (boundAttributes.Length is 0)
                {
                    continue;
                }

                foreach (var boundAttribute in boundAttributes)
                {
                    if (!SymbolEqualityComparer.Default.Equals(
                            boundAttribute.AttributeClass, logMessageAttribute))
                    {
                        continue;
                    }

                    var attributeDetails = ParseLogAttributeDetails(boundAttribute);
                    if (attributeDetails is null)
                    {
                        continue;
                    }

                    yield return attributeDetails;
                }
            }
        }
    }

    public static List<LogMethodParameter> ParseLogMethodParameters(IMethodSymbol methodSymbol)
    {
        var parameters = new List<LogMethodParameter>(methodSymbol.Parameters.Length);
        foreach (var parameter in methodSymbol.Parameters)
        {
            var parameterType = parameter.Type.ToDisplayString(
                SymbolDisplayFormat.FullyQualifiedFormat.WithMiscellaneousOptions(
                    SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier));

            var parsedParameter = new LogMethodParameter(parameter.Name, parameterType);
            parameters.Add(parsedParameter);
        }

        return parameters;
    }

    private static LogAttributeDetails? ParseLogAttributeDetails(AttributeData attributeData)
    {
        if (!attributeData.ConstructorArguments.Any())
        {
            return null;
        }

        if (Enumerable.Any(
                attributeData.ConstructorArguments,
                typedConstant => typedConstant.Kind == TypedConstantKind.Error))
        {
            return null;
        }

        var items = attributeData.ConstructorArguments;
        var result = new LogAttributeDetails
        {
            EventId = items[0].IsNull ? -1 : (int)items[0].Value!,
            Level = items[1].IsNull ? 6 : (int)items[1].Value!,
            Message = items[2].IsNull ? string.Empty : (string)items[2].Value!
        };

        if (attributeData.NamedArguments.Length is 0)
        {
            return result;
        }

        foreach (var namedArgument in attributeData.NamedArguments)
        {
            var typedConstant = namedArgument.Value;
            if (typedConstant.Kind == TypedConstantKind.Error)
            {
                return null;
            }

            var item = namedArgument.Value;
            switch (namedArgument.Key)
            {
                case "EventId":
                    result.EventId = item.IsNull ? -1 : (int)item.Value!;
                    break;
                case "Level":
                    result.Level = item.IsNull ? 6 : (int)item.Value!;
                    break;
                case "Message":
                    result.Message = item.IsNull ? string.Empty : (string)item.Value!;
                    break;
                case "SkipEnabledCheck":
                    result.SkipEnabledCheck = !item.IsNull && (bool)item.Value!;
                    break;
                case "EventName":
                    result.EventName = item.IsNull ? null : (string?)item.Value;
                    break;
            }
        }

        return result;
    }
}
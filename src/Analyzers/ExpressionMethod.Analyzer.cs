using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Analyzers;


[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ExpressionMethodAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => Rules.Expression.All;

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.MethodDeclaration);
    }

    private readonly static IReadOnlySet<string> AllowedExpressionReturnTypes = new HashSet<string>()
    {
        "System.Boolean",
        "System.Byte",
        "System.Char",
        "System.DateTime",
        "System.Decimal",
        "System.Double",
        "System.Enum",
        "System.Guid",
        "System.Int16",
        "System.Int32",
        "System.Int64",
        "System.String",
        "System.UInt16",
        "System.UInt32",
        "System.UInt64",
        "System.Uri",
        "Newtonsoft.Json.Linq.JObject",
    };
    private readonly static string ExpressionAttribute = "Mielek.Azure.ApiManagement.PolicyToolkit.Attributes.ExpressionAttribute";
    private readonly static string ContextParamType = "Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.IContext";
    private readonly static string ContextParamName = "context";

    private static void Analyze(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not MethodDeclarationSyntax method)
        {
            throw new Exception();
        }
        var model = context.SemanticModel;

        var isExpression = method.ContainsAttributeOfType(model, ExpressionAttribute);
        if (!isExpression) return;

        var type = model.GetTypeInfo(method.ReturnType).Type;
        if (type == null)
        {
            var diagnostic = Diagnostic.Create(Rules.Expression.ReturnTypeNotAllowed, method.ReturnType.GetLocation(), method.ReturnType.ToString());
            context.ReportDiagnostic(diagnostic);
        }
        else
        {
            var fullTypeName = type.ToFullyQualifiedString();
            if (!AllowedExpressionReturnTypes.Contains(fullTypeName))
            {
                var diagnostic = Diagnostic.Create(Rules.Expression.ReturnTypeNotAllowed, method.ReturnType.GetLocation(), fullTypeName);
                context.ReportDiagnostic(diagnostic);
            }
        }

        var parameters = method.ParameterList.Parameters;
        if (parameters.Count != 1)
        {
            context.ReportDiagnostic(Diagnostic.Create(Rules.Expression.WrongParameterCount, method.ParameterList.GetLocation(), parameters.Count));
        }
        else
        {
            var parameter = parameters[0];
            var parameterSymbol = model.GetDeclaredSymbol(parameter);

            if (parameterSymbol == null)
            {
                throw new Exception();
            }

            if (parameterSymbol.Type.ToFullyQualifiedString() != ContextParamType)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rules.Expression.WrongParameterType, parameter.Type?.GetLocation(), parameterSymbol.Type.ToFullyQualifiedString(), ContextParamType));
            }

            if (parameter.Identifier.ValueText != ContextParamName)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rules.Expression.WrongParameterName, parameter.Identifier.GetLocation(), parameter.Identifier.ValueText, ContextParamName));
            }
        }
    }

}
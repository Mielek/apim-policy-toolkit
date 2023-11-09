using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Mielek.Analyzers;


[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ExpressionMethodAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => Rules.Expression.All;

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.MethodDeclaration);
        context.EnableConcurrentExecution();
    }

    public void Analyze(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not MethodDeclarationSyntax method)
        {
            throw new Exception();
        }
        var model = context.SemanticModel;

        var isExpression = method.ContainsAttributeOfType(model, Constants.Method.ExpressionAttribute);
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
            if (!Constants.AllowedExpressionReturnTypes.Contains(fullTypeName))
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

            if (parameterSymbol.Type.ToFullyQualifiedString() != Constants.Method.ContextParamType)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rules.Expression.WrongParameterType, parameter.Type?.GetLocation(), parameterSymbol.Type.ToFullyQualifiedString(), Constants.Method.ContextParamType));
            }

            if (parameter.Identifier.ValueText != Constants.Method.ContextParamName)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rules.Expression.WrongParameterName, parameter.Identifier.GetLocation(), parameter.Identifier.ValueText, Constants.Method.ContextParamName));
            }
        }
    }

}
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Mielek.Analyzers;
public class ExpressionMethodAnalyzer
{
    private readonly SyntaxNodeAnalysisContext _context;
    private readonly SemanticModel _semanticModel;
    private readonly MethodDeclarationSyntax _methodDeclaration;

    public ExpressionMethodAnalyzer(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not MethodDeclarationSyntax methodDeclaration)
        {
            throw new Exception();
        }
        _methodDeclaration = methodDeclaration;
        _semanticModel = context.SemanticModel;
        _context = context;
    }

    public void Analyze()
    {
        if (!IsExpression()) return;

        CheckAllowedReturnType();

        var parameters = _methodDeclaration.ParameterList.Parameters;
        if (parameters.Count != 1)
        {
            _context.ReportDiagnostic(Diagnostic.Create(Rules.ParameterLength, _methodDeclaration.ParameterList.GetLocation(), parameters.Count));
            return;
        }

        var parameter = parameters[0];
        var parameterSymbol = _semanticModel.GetDeclaredSymbol(parameter) as IParameterSymbol;

        if (parameterSymbol == null)
        {
            throw new Exception();
        }

        if (parameterSymbol.Type.ToFullyQualifiedString() != Constants.Method.ContextParamType)
        {
            _context.ReportDiagnostic(Diagnostic.Create(Rules.ParameterType, parameter.Type?.GetLocation(), parameterSymbol.Type.ToFullyQualifiedString(), Constants.Method.ContextParamType));
        }

        if (parameter.Identifier.ValueText != Constants.Method.ContextParamName)
        {
            _context.ReportDiagnostic(Diagnostic.Create(Rules.ParameterName, parameter.Identifier.GetLocation(), parameter.Identifier.ValueText, Constants.Method.ContextParamName));
        }

    }

    private bool IsExpression()
    {
        return _methodDeclaration.AttributeLists.SelectMany(a => a.Attributes).Any(attribute =>
        {
            var attributeType = _semanticModel.GetSymbolInfo(attribute).Symbol?.ContainingType;
            return attributeType != null && attributeType.ToFullyQualifiedString() == Constants.Method.ExpressionAttribute;
        });
    }

    public void CheckAllowedReturnType()
    {
        var type = _semanticModel.GetTypeInfo(_methodDeclaration.ReturnType).Type;
        if (type == null)
        {
            var diagnostic = Diagnostic.Create(Rules.ReturnValue, _methodDeclaration.ReturnType.GetLocation(), _methodDeclaration.ReturnType.ToString());
            _context.ReportDiagnostic(diagnostic);
            return;
        }
        var fullTypeName = type.ToFullyQualifiedString();
        if (!Constants.AllowedExpressionReturnTypes.Contains(fullTypeName))
        {
            var diagnostic = Diagnostic.Create(Rules.ReturnValue, _methodDeclaration.ReturnType.GetLocation(), fullTypeName);
            _context.ReportDiagnostic(diagnostic);
        }
    }
}
using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class PolicyDocumentMethodAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => Rules.PolicyDocument.All;

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.MethodDeclaration);
    }

    private readonly static IReadOnlySet<string> AllowedPolicyDocumentReturnTypes = new HashSet<string>()
    {
        "System.Xml.Linq.XElement"
    };
    private const string PolicyDocumentAttribute = "Mielek.Azure.ApiManagement.PolicyToolkit.Attributes.DocumentAttribute";

    private static void Analyze(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not MethodDeclarationSyntax method)
        {
            throw new Exception();
        }
        var model = context.SemanticModel;

        var isDocument = method.ContainsAttributeOfType(model, PolicyDocumentAttribute);
        if (!isDocument) return;

        var type = model.GetTypeInfo(method.ReturnType).Type;
        if (type == null)
        {
            var diagnostic = Diagnostic.Create(Rules.PolicyDocument.ReturnValue, method.ReturnType.GetLocation(), method.ReturnType.ToString());
            context.ReportDiagnostic(diagnostic);
        }
        else
        {
            var fullTypeName = type.ToFullyQualifiedString();
            if (!AllowedPolicyDocumentReturnTypes.Contains(fullTypeName))
            {
                var diagnostic = Diagnostic.Create(Rules.PolicyDocument.ReturnValue, method.ReturnType.GetLocation(), fullTypeName);
                context.ReportDiagnostic(diagnostic);
            }
        }

        var parameters = method.ParameterList.Parameters;
        if (parameters.Count > 0)
        {
            context.ReportDiagnostic(Diagnostic.Create(Rules.PolicyDocument.NoParametersAllowed, method.ParameterList.GetLocation(), parameters.Count));
        }
    }

}
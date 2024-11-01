using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;
using Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;
using Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Syntax;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

public class CSharpPolicyCompiler
{
    private ClassDeclarationSyntax _document;

    private BlockCompiler _blockCompiler;

    public CSharpPolicyCompiler(ClassDeclarationSyntax document)
    {
        _document = document;
        var invStatement = new ExpressionStatementCompiler([
            new AuthenticationBasicCompiler(),
            new AuthenticationCertificateCompiler(),
            new AuthenticationManagedIdentityCompiler(),
            new BaseCompiler(),
            new CacheLookupCompiler(),
            new CacheStoreCompiler(),
            new CheckHeaderCompiler(),
            new CorsCompiler(),
            new ForwardRequestCompiler(),
            new InlinePolicyCompiler(),
            new IpFilterCompiler(),
            new JsonToXmlCompiler(),
            new JsonPCompiler(),
            new MockResponseCompiler(),
            new QuotaCompiler(),
            new RateLimitByKeyCompiler(),
            new RateLimitCompiler(),
            new ReturnResponseCompiler(),
            new RewriteUriCompiler(),
            new SendRequestCompiler(),
            new SetBackendServiceCompiler(),
            new SetBodyCompiler(),
            SetHeaderCompiler.AppendCompiler,
            SetHeaderCompiler.RemoveCompiler,
            SetHeaderCompiler.SetCompiler,
            SetHeaderCompiler.SetIfNotExistCompiler,
            new SetMethodCompiler(),
            SetQueryParameterCompiler.AppendCompiler,
            SetQueryParameterCompiler.RemoveCompiler,
            SetQueryParameterCompiler.SetCompiler,
            SetQueryParameterCompiler.SetIfNotExistCompiler,
            new SetVariableCompiler(),
            new ValidateJwtCompiler(),
            new EmitMetricCompiler(),
            EmitTokenMetricCompiler.Llm,
            EmitTokenMetricCompiler.AzureOpenAi,
            SemanticCacheStoreCompiler.Llm,
            SemanticCacheStoreCompiler.AzureOpenAi,
            SemanticCacheLookupCompiler.Llm,
            SemanticCacheLookupCompiler.AzureOpenAi,
        ]);
        var loc = new LocalDeclarationStatementCompiler([
            new AuthenticationManageIdentityReturnValueCompiler()
        ]);
        _blockCompiler = new([
            invStatement,
            loc
        ]);
        _blockCompiler.AddCompiler(new IfStatementCompiler(_blockCompiler));
    }

    public ICompilationResult Compile()
    {
        var methods = _document.DescendantNodes()
            .OfType<MethodDeclarationSyntax>();
        var policyDocument = new XElement("policies");
        var context = new CompilationContext(_document, policyDocument);

        foreach (var method in methods)
        {
            var sectionName = method.Identifier.ValueText switch
            {
                nameof(IDocument.Inbound) => "inbound",
                nameof(IDocument.Outbound) => "outbound",
                nameof(IDocument.Backend) => "backend",
                nameof(IDocument.OnError) => "on-error",
                _ => string.Empty
            };

            if (string.IsNullOrEmpty(sectionName))
            {
                continue;
            }

            CompileSection(context, sectionName, method);
        }

        return context;
    }


    private void CompileSection(ICompilationContext context, string section, MethodDeclarationSyntax method)
    {
        if (method.Body is null)
        {
            context.ReportError($"Method {section} is not allowed as expression. ({method.GetLocation()})");
            return;
        }

        var sectionElement = new XElement(section);
        var sectionContext = new SubCompilationContext(context, sectionElement);
        _blockCompiler.Compile(sectionContext, method.Body);
        context.AddPolicy(sectionElement);
    }
}
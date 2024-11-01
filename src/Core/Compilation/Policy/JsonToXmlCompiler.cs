using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class JsonToXmlCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.JsonToXml);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<JsonToXmlConfig>(context, "json-to-xml", out var values))
        {
            return;
        }

        var element = new XElement("json-to-xml");
        if (!element.AddAttribute(values, nameof(JsonToXmlConfig.Apply), "apply"))
        {
            context.ReportError($"{nameof(JsonToXmlConfig.Apply)} must be present. {node.GetLocation()}");
            return;
        }

        element.AddAttribute(values, nameof(JsonToXmlConfig.ConsiderAcceptHeader), "consider-accept-header");
        element.AddAttribute(values, nameof(JsonToXmlConfig.ParseDate), "parse-date");
        element.AddAttribute(values, nameof(JsonToXmlConfig.NamespaceSeparator), "namespace-separator");
        element.AddAttribute(values, nameof(JsonToXmlConfig.NamespacePrefix), "namespace-prefix");
        element.AddAttribute(values, nameof(JsonToXmlConfig.AttributeBlockName), "attribute-block-name");

        context.AddPolicy(element);
    }
}
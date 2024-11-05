using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class MockResponseCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.MockResponse);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        var arguments = node.ArgumentList.Arguments;
        if (arguments.Count > 1)
        {
            context.ReportError($"Wrong argument count for mock response policy. {node.GetLocation()}");
            return;
        }

        var element = new XElement("mock-response");
        if (arguments.Count == 1)
        {
            HandleConfig(context, element, arguments[0].Expression.ProcessExpression(context));
        }

        context.AddPolicy(element);
    }

    private void HandleConfig(ICompilationContext context, XElement element, InitializerValue value)
    {
        if (value.Type != nameof(MockResponseConfig))
        {
            context.ReportError(
                $"Mock response policy argument must be of type {nameof(MockResponseConfig)}. {value.Node.GetLocation()}");
            return;
        }

        var values = value.NamedValues;
        if (values is null)
        {
            context.ReportError($"TODO. {value.Node.GetLocation()}");
            return;
        }

        element.AddAttribute(values, nameof(MockResponseConfig.StatusCode), "status-code");
        element.AddAttribute(values, nameof(MockResponseConfig.ContentType), "content-type");
        element.AddAttribute(values, nameof(MockResponseConfig.Index), "index");
    }
}
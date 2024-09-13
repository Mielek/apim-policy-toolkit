using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class ReturnResponseCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.ReturnResponse);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<ReturnResponseConfig>(context, "return-response", out var values))
        {
            return;
        }

        var element = new XElement("return-response");

        element.AddAttribute(values, nameof(ReturnResponseConfig.ResponseVariableName), "response-variable-name");

        if (values.TryGetValue(nameof(ReturnResponseConfig.Status), out var statusConfig))
        {
            HandleStatus(context, element, statusConfig);
        }

        if (values.TryGetValue(nameof(ReturnResponseConfig.Headers), out var headers))
        {
            SetHeaderCompiler.HandleHeaders(context, element, headers);
        }

        if (values.TryGetValue(nameof(ReturnResponseConfig.Body), out var body))
        {
            SetBodyCompiler.HandleBody(context, element, body);
        }

        context.AddPolicy(element);
    }

    private static void HandleStatus(ICompilationContext context, XElement element, InitializerValue status)
    {
        if (!status.TryGetValues<StatusConfig>(out var config))
        {
            context.ReportError($"{nameof(StatusConfig)}. {status.Node.GetLocation()}");
            return;
        }

        var statusElement = new XElement("set-status");

        if (!statusElement.AddAttribute(config, nameof(StatusConfig.Code), "code"))
        {
            context.ReportError($"{nameof(BodyConfig.Content)}. {status.Node.GetLocation()}");
            return;
        }

        statusElement.AddAttribute(config, nameof(StatusConfig.Reason), "reason");
        element.Add(statusElement);
    }
}
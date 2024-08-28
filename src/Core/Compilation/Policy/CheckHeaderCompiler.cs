using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class CheckHeaderCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.CheckHeader);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (node.ArgumentList.Arguments.Count != 1)
        {
            context.ReportError($"Wrong argument count for check-header policy. {node.GetLocation()}");
            return;
        }

        if (node.ArgumentList.Arguments[0].Expression is not ObjectCreationExpressionSyntax config)
        {
            context.ReportError(
                $"Check-header policy argument must be an object creation expression. {node.GetLocation()}");
            return;
        }

        var initializer = config.Process(context);
        if (initializer.Type != nameof(CheckHeaderConfig))
        {
            context.ReportError(
                $"Check-header policy argument must be of type {nameof(CheckHeaderConfig)}. {node.GetLocation()}");
            return;
        }

        var values = initializer.NamedValues;
        if (values is null)
        {
            context.ReportError($"TODO. {node.GetLocation()}");
            return;
        }

        var element = new XElement("check-header");

        if (!element.AddAttribute(values, nameof(CheckHeaderConfig.Name), "name"))
        {
            context.ReportError($"{nameof(CheckHeaderConfig.Name)}. {node.GetLocation()}");
            return;
        }

        if (!element.AddAttribute(values, nameof(CheckHeaderConfig.FailCheckHttpCode), "failed-check-httpcode"))
        {
            context.ReportError($"{nameof(CheckHeaderConfig.FailCheckHttpCode)}. {node.GetLocation()}");
            return;
        }

        if (!element.AddAttribute(values, nameof(CheckHeaderConfig.FailCheckErrorMessage),
                "failed-check-error-message"))
        {
            context.ReportError($"{nameof(CheckHeaderConfig.FailCheckErrorMessage)}. {node.GetLocation()}");
            return;
        }

        if (!element.AddAttribute(values, nameof(CheckHeaderConfig.IgnoreCase), "ignore-case"))
        {
            context.ReportError($"{nameof(CheckHeaderConfig.IgnoreCase)}. {node.GetLocation()}");
            return;
        }

        if (!values.TryGetValue(nameof(CheckHeaderConfig.Values), out var headerValues))
        {
            context.ReportError($"{nameof(CheckHeaderConfig.Values)}. {node.GetLocation()}");
            return;
        }

        var elements = (headerValues.UnnamedValues ?? [])
            .Select(origin => new XElement("value", origin.Value!))
            .ToArray<object>();
        if (elements.Length == 0)
        {
            context.ReportError(
                $"{nameof(CheckHeaderConfig.Values)} must have at least one value. {node.GetLocation()}");
            return;
        }

        element.Add(elements);
        
        context.AddPolicy(element);
    }
}
using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class RateLimitCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.RateLimit);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (node.ArgumentList.Arguments.Count != 1)
        {
            context.ReportError($"Wrong argument count for rate limit policy. {node.GetLocation()}");
            return;
        }

        if (node.ArgumentList.Arguments[0].Expression is not ObjectCreationExpressionSyntax config)
        {
            context.ReportError($"Rate limit policy argument must be an object creation expression. {node.GetLocation()}");
            return;
        }

        var initializer = config.Process(context);
        if (initializer.Type != nameof(RateLimitConfig))
        {
            context.ReportError($"Rate limit policy argument must be of type {nameof(RateLimitConfig)}. {node.GetLocation()}");
            return;
        }
        if (initializer.NamedValues is null)
        {
            context.ReportError($"TODO. {node.GetLocation()}");
            return;
        }

        var element = new XElement("rate-limit");

        if (initializer.NamedValues.TryGetValue(nameof(RateLimitConfig.Calls), out var rateLimitCalls))
        {
            element.Add(new XAttribute("calls", rateLimitCalls.Value!));
        }
        else
        {
            context.ReportError($"TODO. {node.GetLocation()}");
            return;
        }

        if (initializer.NamedValues.TryGetValue(nameof(RateLimitConfig.RenewalPeriod), out var rateLimitRenewal))
        {
            element.Add(new XAttribute("renewal-period", rateLimitRenewal.Value!));
        }
        else
        {
            context.ReportError($"TODO. {node.GetLocation()}");
            return;
        }

        if (initializer.NamedValues.TryGetValue(nameof(RateLimitConfig.RetryAfterHeaderName), out var retryAfterHeader))
        {
            element.Add(new XAttribute("retry-after-header-name", retryAfterHeader.Value!));
        }

        if (initializer.NamedValues.TryGetValue(nameof(RateLimitConfig.RetryAfterVariableName), out var retryAfterVariable))
        {
            element.Add(new XAttribute("retry-after-variable-name", retryAfterVariable.Value!));
        }

        if (initializer.NamedValues.TryGetValue(nameof(RateLimitConfig.RemainingCallsHeaderName), out var remainingCallsHeader))
        {
            element.Add(new XAttribute("remaining-calls-header-name", remainingCallsHeader.Value!));
        }

        if (initializer.NamedValues.TryGetValue(nameof(RateLimitConfig.RemainingCallsVariableName), out var remainingCallsVariable))
        {
            element.Add(new XAttribute("remaining-calls-variable-name", remainingCallsVariable.Value!));
        }

        if (initializer.NamedValues.TryGetValue(nameof(RateLimitConfig.TotalCallsHeaderName), out var totalCallsHeader))
        {
            element.Add(new XAttribute("total-calls-header-name", totalCallsHeader.Value!));
        }

        if (initializer.NamedValues.TryGetValue(nameof(RateLimitConfig.Apis), out var apis))
        {
            foreach (var api in apis.UnnamedValues!)
            {
                if (!Handle(context, "api", api, out var apiElement))
                {
                    continue;
                }

                if (api.NamedValues!.TryGetValue(nameof(ApiRateLimit.Operations), out var operations))
                {
                    foreach (var operation in operations.UnnamedValues!)
                    {
                        if (!Handle(context, "operation", operation, out var operationElement))
                        {
                            continue;
                        }
                        apiElement.Add(operationElement);
                    }
                }

                element.Add(apiElement);
            }

        }

        context.AddPolicy(element);
    }

    private bool Handle(ICompilationContext context, string name, InitializerValue value, out XElement element)
    {
        element = new XElement(name);
        var values = value.NamedValues!;
        if (values.TryGetValue(nameof(EntityLimitConfig.Name), out var apiName))
        {
            element.Add(new XAttribute("name", apiName.Value!));
        }

        if (values.TryGetValue(nameof(EntityLimitConfig.Id), out var apiId))
        {
            element.Add(new XAttribute("id", apiId.Value!));
        }

        if (apiName is null && apiId is null)
        {
            context.ReportError($"TODO. {value.Node.GetLocation()}");
            return false;
        }

        if (values.TryGetValue(nameof(EntityLimitConfig.Calls), out var apiCalls))
        {
            element.Add(new XAttribute("calls", apiCalls.Value!));
        }
        else
        {
            context.ReportError($"TODO. {value.Node.GetLocation()}");
            return false;
        }

        if (values.TryGetValue(nameof(EntityLimitConfig.RenewalPeriod), out var apiRenewal))
        {
            element.Add(new XAttribute("renewal-period", apiRenewal.Value!));
        }
        else
        {
            context.ReportError($"TODO. {value.Node.GetLocation()}");
            return false;
        }

        return true;
    }
}
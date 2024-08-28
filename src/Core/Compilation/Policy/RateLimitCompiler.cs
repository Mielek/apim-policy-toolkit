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

        var values = initializer.NamedValues;
        if (values is null)
        {
            context.ReportError($"No initializer. {node.GetLocation()}");
            return;
        }

        var element = new XElement("rate-limit");

        if (!element.AddAttribute(values, nameof(RateLimitConfig.Calls), "calls"))
        {
            context.ReportError($"{nameof(RateLimitConfig.Calls)}. {node.GetLocation()}");
            return;
        }

        if (!element.AddAttribute(values, nameof(RateLimitConfig.RenewalPeriod), "renewal-period"))
        {
            context.ReportError($"{nameof(RateLimitConfig.RenewalPeriod)}. {node.GetLocation()}");
            return;
        }

        element.AddAttribute(values, nameof(RateLimitConfig.RetryAfterHeaderName), "retry-after-header-name");
        element.AddAttribute(values, nameof(RateLimitConfig.RetryAfterVariableName), "retry-after-variable-name");
        element.AddAttribute(values, nameof(RateLimitConfig.RemainingCallsHeaderName), "remaining-calls-header-name");
        element.AddAttribute(values, nameof(RateLimitConfig.RemainingCallsVariableName), "remaining-calls-variable-name");
        element.AddAttribute(values, nameof(RateLimitConfig.TotalCallsHeaderName), "total-calls-header-name");

        if (values.TryGetValue(nameof(RateLimitConfig.Apis), out var apis))
        {
            foreach (var api in apis.UnnamedValues!)
            {
                if(!Handle(context, "api", api, out var apiElement))
                {
                    continue;
                }
                element.Add(apiElement);

                if (api.NamedValues!.TryGetValue(nameof(ApiRateLimit.Operations), out var operations))
                {
                    foreach (var operation in operations.UnnamedValues!)
                    {
                        if(Handle(context, "operation", operation, out var operationElement))
                        {
                            apiElement.Add(operationElement);
                        }
                    }
                }
            }
        }

        context.AddPolicy(element);
    }

    private bool Handle(ICompilationContext context, string name, InitializerValue value, out XElement element)
    {
        element = new XElement(name);
        var values = value.NamedValues!;
        
        var isNameAdded = element.AddAttribute(values, nameof(EntityLimitConfig.Name), "name");
        var isIdAdded = element.AddAttribute(values, nameof(EntityLimitConfig.Id), "id");

        if (!isNameAdded && !isIdAdded)
        {
            context.ReportError($"{nameof(EntityLimitConfig.Name)} && {nameof(EntityLimitConfig.Id)}. {value.Node.GetLocation()}");
            return false;
        }

        if (!element.AddAttribute(values, nameof(EntityLimitConfig.Calls), "calls"))
        {
            context.ReportError($"{nameof(EntityLimitConfig.Calls)}. {value.Node.GetLocation()}");
            return false;
        }

        if (!element.AddAttribute(values, nameof(EntityLimitConfig.RenewalPeriod), "renewal-period"))
        {
            context.ReportError($"{nameof(EntityLimitConfig.RenewalPeriod)}. {value.Node.GetLocation()}");
            return false;
        }

        return true;
    }
}
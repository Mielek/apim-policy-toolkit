using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class RateLimitByKeyCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.RateLimitByKey);
    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<RateLimitByKeyConfig>(context, "rate-limit-by-key", out var values))
        {
            return;
        }

        var element = new XElement("rate-limit-by-key");

        if (!element.AddAttribute(values, nameof(RateLimitByKeyConfig.Calls), "calls"))
        {
            context.ReportError($"{nameof(RateLimitByKeyConfig.Calls)}. {node.GetLocation()}");
            return;
        }

        if (!element.AddAttribute(values, nameof(RateLimitByKeyConfig.RenewalPeriod), "renewal-period"))
        {
            context.ReportError($"{nameof(RateLimitConfig.RenewalPeriod)}. {node.GetLocation()}");
            return;
        }

        if (!element.AddAttribute(values, nameof(RateLimitByKeyConfig.CounterKey), "counter-key"))
        {
            context.ReportError($"{nameof(RateLimitByKeyConfig.CounterKey)}. {node.GetLocation()}");
            return;
        }
        
        element.AddAttribute(values, nameof(RateLimitByKeyConfig.IncrementCondition), "increment-condition");
        element.AddAttribute(values, nameof(RateLimitByKeyConfig.IncrementCount), "increment-count");
        element.AddAttribute(values, nameof(RateLimitByKeyConfig.RetryAfterHeaderName), "retry-after-header-name");
        element.AddAttribute(values, nameof(RateLimitByKeyConfig.RetryAfterVariableName), "retry-after-variable-name");
        element.AddAttribute(values, nameof(RateLimitByKeyConfig.RemainingCallsHeaderName), "remaining-calls-header-name");
        element.AddAttribute(values, nameof(RateLimitByKeyConfig.RemainingCallsVariableName), "remaining-calls-variable-name");
        element.AddAttribute(values, nameof(RateLimitByKeyConfig.TotalCallsHeaderName), "total-calls-header-name");
        
        context.AddPolicy(element);
    }
}
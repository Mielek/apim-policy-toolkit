using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class RateLimitByKeyCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.RateLimitByKey);
    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (node.ArgumentList.Arguments.Count != 1)
        {
            context.ReportError($"Wrong argument count for rate limit by key policy. {node.GetLocation()}");
            return;
        }

        if (node.ArgumentList.Arguments[0].Expression is not ObjectCreationExpressionSyntax config)
        {
            context.ReportError($"Rate limit by key policy argument must be an object creation expression. {node.GetLocation()}");
            return;
        }

        var initializer = config.Process(context);
        if (initializer.Type != nameof(RateLimitByKeyConfig))
        {
            context.ReportError($"Rate limit by key policy argument must be of type {nameof(RateLimitByKeyConfig)}. {node.GetLocation()}");
            return;
        }

        var values = initializer.NamedValues;
        if (values is null)
        {
            context.ReportError($"No initializer. {node.GetLocation()}");
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
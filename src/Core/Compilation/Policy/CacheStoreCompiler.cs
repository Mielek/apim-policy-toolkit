using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class CacheStoreCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IOutboundContext.CacheStore);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        var arguments = node.ArgumentList.Arguments;
        if (arguments.Count is > 2 or < 1)
        {
            context.ReportError($"Wrong argument count for cache-store policy. {node.GetLocation()}");
            return;
        }

        var element = new XElement("cache-store");

        element.Add(new XAttribute("duration", arguments[0].Expression.ProcessParameter(context)));

        if (arguments.Count == 2)
        {
            element.Add(new XAttribute("cache-response", arguments[1].Expression.ProcessParameter(context)));
        }

        context.AddPolicy(element);
    }
}
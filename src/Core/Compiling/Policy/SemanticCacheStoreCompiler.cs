// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class SemanticCacheStoreCompiler : IMethodPolicyHandler
{
    public static IMethodPolicyHandler Llm =>
        new SemanticCacheStoreCompiler(nameof(IOutboundContext.LlmSemanticCacheStore), "llm-semantic-cache-store");

    public static IMethodPolicyHandler AzureOpenAi =>
        new SemanticCacheStoreCompiler(nameof(IOutboundContext.AzureOpenAiSemanticCacheStore),
            "azure-openai-semantic-cache-store");

    private readonly string _policyName;
    public string MethodName { get; }

    private SemanticCacheStoreCompiler(string methodName, string policyName)
    {
        MethodName = methodName;
        _policyName = policyName;
    }

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        var arguments = node.ArgumentList.Arguments;
        if (arguments.Count != 1)
        {
            context.ReportError($"Wrong argument count for {_policyName} policy. {node.GetLocation()}");
            return;
        }

        var element = new XElement(_policyName);
        element.Add(new XAttribute("duration", arguments[0].Expression.ProcessParameter(context)));
        context.AddPolicy(element);
    }
}
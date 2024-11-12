// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class SemanticCacheLookupCompiler : IMethodPolicyHandler
{
    public static IMethodPolicyHandler Llm => new SemanticCacheLookupCompiler(
        nameof(IInboundContext.LlmSemanticCacheLookup), "llm-semantic-cache-lookup");

    public static IMethodPolicyHandler AzureOpenAi => new SemanticCacheLookupCompiler(
        nameof(IInboundContext.AzureOpenAiSemanticCacheLookup), "azure-openai-semantic-cache-lookup");

    private readonly string _policyName;
    public string MethodName { get; }

    SemanticCacheLookupCompiler(string methodName, string policyName)
    {
        MethodName = methodName;
        _policyName = policyName;
    }

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<SemanticCacheLookupConfig>(context, _policyName, out var values))
        {
            return;
        }

        var element = new XElement(_policyName);

        if (!element.AddAttribute(values, nameof(SemanticCacheLookupConfig.ScoreThreshold), "score-threshold"))
        {
            context.ReportError(
                $"{_policyName} {nameof(SemanticCacheLookupConfig.ScoreThreshold)} score-threshold. {node.GetLocation()}");
            return;
        }

        if (!element.AddAttribute(values, nameof(SemanticCacheLookupConfig.EmbeddingsBackendId),
                "embeddings-backend-id"))
        {
            context.ReportError(
                $"{_policyName} {nameof(SemanticCacheLookupConfig.EmbeddingsBackendId)} embeddings-backend-id. {node.GetLocation()}");
            return;
        }

        if (!element.AddAttribute(values, nameof(SemanticCacheLookupConfig.EmbeddingsBackendAuth),
                "embeddings-backend-auth"))
        {
            context.ReportError(
                $"{_policyName} {nameof(SemanticCacheLookupConfig.EmbeddingsBackendAuth)} embeddings-backend-auth. {node.GetLocation()}");
            return;
        }

        element.AddAttribute(values, nameof(SemanticCacheLookupConfig.IgnoreSystemMessages), "ignore-system-messages");
        element.AddAttribute(values, nameof(SemanticCacheLookupConfig.MaxMessageCount), "max-message-count");

        if (values.TryGetValue(nameof(SemanticCacheLookupConfig.VaryBy), out var varyByInitializer))
        {
            foreach (var varyBy in varyByInitializer.UnnamedValues ?? [])
            {
                if (varyBy.Value is not null)
                {
                    element.Add(new XElement("vary-by", varyBy.Value));
                }
                else
                {
                    context.ReportError(
                        $"{_policyName} {nameof(SemanticCacheLookupConfig.VaryBy)} vary-by. {varyBy.Node.GetLocation()}");
                }
            }
        }

        context.AddPolicy(element);
    }
}
using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class ForwardRequestCompiler : IMethodPolicyHandler
{
    readonly static IReadOnlyDictionary<string, string> FieldToAttribute = new Dictionary<string, string>
    {
        { nameof(ForwardRequestConfig.Timeout), "timeout" },
        { nameof(ForwardRequestConfig.TimeoutMs), "timeout-ms" },
        { nameof(ForwardRequestConfig.ContinueTimeout), "continue-timeout" },
        { nameof(ForwardRequestConfig.HttpVersion), "http-version" },
        { nameof(ForwardRequestConfig.FollowRedirects), "follow-redirects" },
        { nameof(ForwardRequestConfig.BufferRequestBody), "buffer-request-body" },
        { nameof(ForwardRequestConfig.BufferResponse), "buffer-response" },
        { nameof(ForwardRequestConfig.FailOnErrorStatusCode), "fail-on-error-status-code" }
    };
    
    public string MethodName => nameof(IBackendContext.ForwardRequest);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (node.ArgumentList.Arguments.Count > 1)
        {
            context.ReportError($"Wrong argument count for forward request policy. {node.GetLocation()}");
            return;
        }

        var element = new XElement("forward-request");
        if (node.ArgumentList.Arguments.Count == 1)
        {
            if (node.ArgumentList.Arguments[0].Expression is not ObjectCreationExpressionSyntax config)
            {
                context.ReportError($"Forward request policy argument must be an object creation expression. {node.GetLocation()}");
                return;
            }
            
            if(config.Type is IdentifierNameSyntax identifier && identifier.Identifier.Text != "ForwardRequestConfig")
            {
                context.ReportError($"Forward request policy argument must be of type ForwardRequestConfig. {node.GetLocation()}");
                return;
            }

            if (config.Initializer is not null)
            {
                var paramsDictionary = config.Initializer.ProcessInitializerExpression(context, FieldToAttribute);
                
                if(paramsDictionary.ContainsKey("timeout") && paramsDictionary.ContainsKey("timeout-ms"))
                {
                    context.ReportError($"Forward request policy cannot have both timeout and timeout-ms. {node.GetLocation()}");
                }
                
                foreach ((string key, string value) in paramsDictionary)
                {
                    element.Add(new XAttribute(key, value));
                }
            }
        }

        context.AddPolicy(element);
    }
}
using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class CorsCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.Cors);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (node.ArgumentList.Arguments.Count != 1)
        {
            context.ReportError($"Wrong argument count for cors policy. {node.GetLocation()}");
            return;
        }

        if (node.ArgumentList.Arguments[0].Expression is not ObjectCreationExpressionSyntax config)
        {
            context.ReportError($"Cors policy argument must be an object creation expression. {node.GetLocation()}");
            return;
        }

        var initializer = config.Process(context);
        if (initializer.Type != "CorsConfig")
        {
            context.ReportError($"Cors policy argument must be of type CorsConfig. {node.GetLocation()}");
            return;
        }

        var element = new XElement("cors");

        if (initializer.NamedValues is null)
        {
            context.ReportError($"TODO. {node.GetLocation()}");
            return;
        }

        if (initializer.NamedValues.TryGetValue(nameof(CorsConfig.AllowCredentials), out var allowCredentials))
        {
            element.Add(new XAttribute("allow-credentials", allowCredentials.Value!));
        }

        if (initializer.NamedValues.TryGetValue(nameof(CorsConfig.TerminateUnmatchedRequest),
                out var terminateUnmatchedRequest))
        {
            element.Add(new XAttribute("terminate-unmatched-request", terminateUnmatchedRequest.Value!));
        }

        if (initializer.NamedValues.TryGetValue(nameof(CorsConfig.AllowedOrigins), out var allowedOrigins))
        {
            var allowedOriginsElement = new XElement("allowed-origins");
            foreach (var origin in allowedOrigins.UnnamedValues ?? [])
            {
                allowedOriginsElement.Add(new XElement("origin", origin.Value!));
            }

            element.Add(allowedOriginsElement);
        }
        else
        {
            context.ReportError($"Cors policy argument must be of type CorsConfig. {node.GetLocation()}");
            return;
        }

        if (initializer.NamedValues.TryGetValue(nameof(CorsConfig.AllowedHeaders), out var allowedHeaders))
        {
            var allowedHeadersElement = new XElement("allowed-headers");
            foreach (var header in allowedHeaders.UnnamedValues ?? [])
            {
                allowedHeadersElement.Add(new XElement("header", header.Value!));
            }

            element.Add(allowedHeadersElement);
        }
        else
        {
            context.ReportError($". {node.GetLocation()}");
            return;
        }

        if (initializer.NamedValues.TryGetValue(nameof(CorsConfig.AllowedMethods), out var allowedMethods))
        {
            var allowedMethodsElement = new XElement("allowed-methods");

            if (initializer.NamedValues.TryGetValue(nameof(CorsConfig.PreflightResultMaxAge), out var maxAge))
            {
                allowedMethodsElement.Add(new XAttribute("preflight-result-max-age", maxAge.Value!));
            }

            foreach (var method in allowedMethods.UnnamedValues ?? [])
            {
                allowedMethodsElement.Add(new XElement("method", method.Value!));
            }

            element.Add(allowedMethodsElement);
        }

        if (initializer.NamedValues.TryGetValue(nameof(CorsConfig.ExposeHeaders), out var exposeHeaders))
        {
            var exposeHeadersElement = new XElement("expose-headers");
            foreach (var header in exposeHeaders.UnnamedValues ?? [])
            {
                exposeHeadersElement.Add(new XElement("header", header.Value!));
            }

            element.Add(exposeHeadersElement);
        }

        context.AddPolicy(element);
    }
}
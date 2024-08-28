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
        if (initializer.Type != nameof(CorsConfig))
        {
            context.ReportError($"Cors policy argument must be of type {nameof(CorsConfig)}. {node.GetLocation()}");
            return;
        }

        var values = initializer.NamedValues;
        if (values is null)
        {
            context.ReportError($"TODO. {node.GetLocation()}");
            return;
        }

        var element = new XElement("cors");

        element.AddAttribute(values, nameof(CorsConfig.AllowCredentials), "allow-credentials");
        element.AddAttribute(values, nameof(CorsConfig.TerminateUnmatchedRequest), "terminate-unmatched-request");

        if (!values.TryGetValue(nameof(CorsConfig.AllowedOrigins), out var allowedOrigins))
        {
            context.ReportError($"{nameof(CorsConfig.AllowedOrigins)}. {node.GetLocation()}");
            return;
        }

        var allowedOriginsElement = new XElement("allowed-origins");
        var origins = (allowedOrigins.UnnamedValues ?? []).Select(origin => new XElement("origin", origin.Value!));
        allowedOriginsElement.Add(origins.ToArray<object>());
        element.Add(allowedOriginsElement);

        if (!values.TryGetValue(nameof(CorsConfig.AllowedHeaders), out var allowedHeaders))
        {
            context.ReportError($"{nameof(CorsConfig.AllowedHeaders)}. {node.GetLocation()}");
            return;
        }

        var headers = (allowedHeaders.UnnamedValues ?? [])
            .Select(origin => new XElement("header", origin.Value!))
            .ToArray<object>();
        element.Add(new XElement("allowed-headers", headers));

        if (values.TryGetValue(nameof(CorsConfig.AllowedMethods), out var allowedMethods))
        {
            var allowedMethodsElement = new XElement("allowed-methods");
            allowedMethodsElement.AddAttribute(values, nameof(CorsConfig.PreflightResultMaxAge), "preflight-result-max-age");
            var methods = (allowedMethods.UnnamedValues ?? [])
                .Select(m => new XElement("method", m.Value!))
                .ToArray<object>();
            allowedMethodsElement.Add(methods);
            element.Add(allowedMethodsElement);
        }

        if (values.TryGetValue(nameof(CorsConfig.ExposeHeaders), out var exposeHeaders))
        {
            var exposeHeadersElements = (exposeHeaders.UnnamedValues ?? [])
                .Select(h => new XElement("header", h.Value!))
                .ToArray<object>();
            element.Add(new XElement("expose-headers", exposeHeadersElements));
        }

        context.AddPolicy(element);
    }
}
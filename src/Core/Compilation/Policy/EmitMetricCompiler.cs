using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class EmitMetricCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.EmitMetric);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<EmitMetricConfig>(context, "emit-metric", out var values))
        {
            return;
        }

        var element = new XElement("emit-metric");
        if (!element.AddAttribute(values, nameof(EmitMetricConfig.Name), "name"))
        {
            context.ReportError($"emit-metric {nameof(EmitMetricConfig.Name)}. {node.GetLocation()}");
            return;
        }

        element.AddAttribute(values, nameof(EmitMetricConfig.Value), "value");
        element.AddAttribute(values, nameof(EmitMetricConfig.Namespace), "namespace");

        if (!values.TryGetValue(nameof(EmitMetricConfig.Dimensions), out var dimensionsInitializer))
        {
            context.ReportError(
                $"emit-metric {nameof(EmitMetricConfig.Dimensions)} must have been defined. {node.GetLocation()}");
            return;
        }

        var dimensions = dimensionsInitializer.UnnamedValues ?? Array.Empty<InitializerValue>();
        if (dimensions.Count == 0)
        {
            context.ReportError(
                $"emit-metric {nameof(EmitMetricConfig.Dimensions)} must have at least one value. {node.GetLocation()}");
            return;
        }

        foreach (var dimension in dimensions)
        {
            if (!dimension.TryGetValues<MetricDimensionConfig>(out var result))
            {
                continue;
            }

            var dimensionElement = new XElement("dimension");
            if (!dimensionElement.AddAttribute(result, nameof(MetricDimensionConfig.Name), "name"))
            {
                context.ReportError(
                    $"emit-metric.dimension {nameof(MetricDimensionConfig.Name)}. {node.GetLocation()}");
                continue;
            }

            dimensionElement.AddAttribute(result, nameof(MetricDimensionConfig.Value), "value");
            element.Add(dimensionElement);
        }
        
        context.AddPolicy(element);
    }
}
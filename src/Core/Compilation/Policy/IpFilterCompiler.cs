using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class IpFilterCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.IpFilter);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (node.ArgumentList.Arguments.Count != 1)
        {
            context.ReportError($"Wrong argument count for IP filter policy. {node.GetLocation()}");
            return;
        }

        if (node.ArgumentList.Arguments[0].Expression is not ObjectCreationExpressionSyntax config)
        {
            context.ReportError(
                $"IP filter policy argument must be an object creation expression. {node.GetLocation()}");
            return;
        }

        var initializer = config.Process(context);

        if (initializer.Type != nameof(IpFilterConfig))
        {
            context.ReportError(
                $"IP filter policy argument must be of type {nameof(IpFilterConfig)}. {node.GetLocation()}");
            return;
        }

        var values = initializer.NamedValues;
        if (values is null)
        {
            context.ReportError($"No initializer. {node.GetLocation()}");
            return;
        }

        var element = new XElement("ip-filter");

        if (!element.AddAttribute(values, nameof(IpFilterConfig.Action), "action"))
        {
            context.ReportError($"{nameof(IpFilterConfig.Action)}. {node.GetLocation()}");
            return;
        }

        bool atLeastOneAddress = false;
        if (values.TryGetValue(nameof(IpFilterConfig.Addresses), out var addresses))
        {
            foreach (var address in addresses.UnnamedValues!)
            {
                element.Add(new XElement("address", address.Value!));
                atLeastOneAddress = true;
            }
        }

        bool atLeastOneRange = false;
        if (values.TryGetValue(nameof(IpFilterConfig.AddressRanges), out var ranges))
        {
            foreach (var range in ranges.UnnamedValues!)
            {
                if (range.Type != nameof(AddressRange))
                {
                    context.ReportError(
                        $"Address range argument must be of type {nameof(AddressRange)}. {range.Node.GetLocation()}");
                    continue;
                }

                var rangeValues = range.NamedValues;
                if (rangeValues is null)
                {
                    context.ReportError($"No initializer. {node.GetLocation()}");
                    return;
                }

                var rangeElement = new XElement("address-range");
                if (!rangeElement.AddAttribute(rangeValues, nameof(AddressRange.From), "from"))
                {
                    context.ReportError($"{nameof(AddressRange.From)}. {range.Node.GetLocation()}");
                    continue;
                }

                if (!rangeElement.AddAttribute(rangeValues, nameof(AddressRange.To), "to"))
                {
                    context.ReportError($"{nameof(AddressRange.To)}. {range.Node.GetLocation()}");
                    continue;
                }

                element.Add(rangeElement);

                atLeastOneRange = true;
            }
        }

        if (!atLeastOneAddress && !atLeastOneRange)
        {
            context.ReportError($"Ip filter elements. {node.GetLocation()}");
            return;
        }

        context.AddPolicy(element);
    }
}
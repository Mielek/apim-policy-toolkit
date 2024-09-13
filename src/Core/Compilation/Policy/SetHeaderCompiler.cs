using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;
using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using static Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies.SetHeaderPolicyBuilder;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class SetHeaderCompiler : IMethodPolicyHandler
{
    public static SetHeaderCompiler AppendCompiler =>
        new SetHeaderCompiler(nameof(IInboundContext.AppendHeader), "append");

    public static SetHeaderCompiler SetCompiler =>
        new SetHeaderCompiler(nameof(IInboundContext.SetHeader), "override");

    public static SetHeaderCompiler SetIfNotExistCompiler =>
        new SetHeaderCompiler(nameof(IInboundContext.SetHeaderIfNotExist), "skip");

    public static SetHeaderCompiler RemoveCompiler =>
        new SetHeaderCompiler(nameof(IInboundContext.RemoveHeader), "delete");

    readonly string _type;

    private SetHeaderCompiler(string methodName, string type)
    {
        MethodName = methodName;
        _type = type;
    }

    public string MethodName { get; }

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        var arguments = node.ArgumentList.Arguments;
        if (_type != "delete" && arguments.Count < 2 ||
            _type == "delete" && arguments.Count != 1)
        {
            context.ReportError($"Wrong argument count for set-header policy. {node.GetLocation()}");
            context.AddPolicy(new XComment("Issue: set-header"));
            return;
        }

        var element = new XElement("set-header");

        var name = node.ArgumentList.Arguments[0].Expression.ProcessParameter(context);
        element.Add(new XAttribute("name", name));
        element.Add(new XAttribute("exists-action", _type));

        for (int i = 1; i < arguments.Count; i++)
        {
            element.Add(new XElement("value", arguments[i].Expression.ProcessParameter(context)));
        }

        context.AddPolicy(element);
    }

    public static void HandleHeaders(ICompilationContext context, XElement root, InitializerValue headers)
    {
        foreach (var header in headers.UnnamedValues!)
        {
            if (!header.TryGetValues<HeaderConfig>(out var headerValues))
            {
                context.ReportError($"{nameof(HeaderConfig)}. {header.Node.GetLocation()}");
                continue;
            }

            var headerElement = new XElement("set-header");
            if (!headerElement.AddAttribute(headerValues, nameof(HeaderConfig.Name), "name"))
            {
                continue;
            }

            headerElement.AddAttribute(headerValues, nameof(HeaderConfig.ExistsAction), "exists-action");

            if (headerValues.TryGetValue(nameof(HeaderConfig.Values), out var values) &&
                values.UnnamedValues is not null)
            {
                foreach (var value in values.UnnamedValues)
                {
                    headerElement.Add(new XElement("value", value.Value!));
                }
            }

            root.Add(headerElement);
        }
    }
}
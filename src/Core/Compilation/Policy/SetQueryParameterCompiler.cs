// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class SetQueryParameterCompiler : IMethodPolicyHandler
{
    public static SetQueryParameterCompiler AppendCompiler =>
        new SetQueryParameterCompiler(nameof(IInboundContext.AppendQueryParameter), "append");

    public static SetQueryParameterCompiler SetCompiler =>
        new SetQueryParameterCompiler(nameof(IInboundContext.SetQueryParameter), "override");

    public static SetQueryParameterCompiler SetIfNotExistCompiler =>
        new SetQueryParameterCompiler(nameof(IInboundContext.SetQueryParameterIfNotExist), "skip");

    public static SetQueryParameterCompiler RemoveCompiler =>
        new SetQueryParameterCompiler(nameof(IInboundContext.RemoveQueryParameter), "delete");

    readonly string _type;

    private SetQueryParameterCompiler(string methodName, string type)
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
            context.ReportError($"Wrong argument count for set-query-parameter policy. {node.GetLocation()}");
            return;
        }

        var element = new XElement("set-query-parameter");

        var name = node.ArgumentList.Arguments[0].Expression.ProcessParameter(context);
        element.Add(new XAttribute("name", name));
        element.Add(new XAttribute("exists-action", _type));

        for (int i = 1; i < arguments.Count; i++)
        {
            element.Add(new XElement("value", arguments[i].Expression.ProcessParameter(context)));
        }

        context.AddPolicy(element);
    }
}
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class JsonPCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IOutboundContext.JsonP);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        var arguments = node.ArgumentList.Arguments;
        if (arguments.Count != 1)
        {
            context.ReportError($"Wrong argument count for jsonp policy. {node.GetLocation()}");
            return;
        }

        var value = node.ArgumentList.Arguments[0].Expression.ProcessParameter(context);
        context.AddPolicy(new XElement("jsonp", new XAttribute("callback-parameter-name", value)));
    }
}
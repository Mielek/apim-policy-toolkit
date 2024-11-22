// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Diagnostics;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class SetMethodCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.SetMethod);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (node.ArgumentList.Arguments.Count != 1)
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.ArgumentCountMissMatchForPolicy,
                node.ArgumentList.GetLocation(),
                "set-method"));
            return;
        }

        var value = node.ArgumentList.Arguments[0].Expression.ProcessParameter(context);
        context.AddPolicy(new XElement("set-method", value));
    }
}
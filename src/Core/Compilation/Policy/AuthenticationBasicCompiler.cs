// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compilation.Policy;

public class AuthenticationBasicCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.AuthenticationBasic);
    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (node.ArgumentList.Arguments.Count != 2)
        {
            context.ReportError($"Wrong argument count for authentication-basic policy. {node.GetLocation()}");
            return;
        }

        var username = node.ArgumentList.Arguments[0].Expression.ProcessParameter(context);
        var password = node.ArgumentList.Arguments[1].Expression.ProcessParameter(context);
        context.AddPolicy(new XElement("authentication-basic", new XAttribute("username", username), new XAttribute("password", password)));
    }
}
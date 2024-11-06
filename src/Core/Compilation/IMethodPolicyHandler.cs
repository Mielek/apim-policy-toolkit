// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compilation;

public interface IMethodPolicyHandler
{
    string MethodName { get; }
    void Handle(ICompilationContext context, InvocationExpressionSyntax node);
}
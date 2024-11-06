// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Azure.ApiManagement.PolicyToolkit.Compilation;

public interface ISyntaxCompiler
{
    SyntaxKind Syntax { get; }

    void Compile(ICompilationContext context, SyntaxNode node);
}
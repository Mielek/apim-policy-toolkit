// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Microsoft.CodeAnalysis;

namespace Azure.ApiManagement.PolicyToolkit.Compiling;

public interface ICompilationContext
{
    void AddPolicy(XNode element);
    void ReportError(string message);

    SyntaxNode SyntaxRoot { get; }
}
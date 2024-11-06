// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Azure.ApiManagement.PolicyToolkit.Authoring;

public interface IHaveExpressionContext
{
    IExpressionContext ExpressionContext { get; }
}
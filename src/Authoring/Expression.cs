// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Azure.ApiManagement.PolicyToolkit.Authoring;

public delegate T Expression<out T>(IExpressionContext context);
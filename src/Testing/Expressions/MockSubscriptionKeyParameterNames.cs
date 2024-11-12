// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Expressions;

public class MockSubscriptionKeyParameterNames : ISubscriptionKeyParameterNames
{
    public string Header { get; set; } = "X-Sub-Header";
    public string Query { get; set; } = "subQuery";
}
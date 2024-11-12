// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Expressions;

public class MockPrivateEndpointConnection : IPrivateEndpointConnection
{
    public string Name { get; set; }
    public string GroupId { get; set; }
    public string MemberName { get; set; }
}
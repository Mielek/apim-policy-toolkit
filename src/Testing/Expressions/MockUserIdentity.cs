// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Expressions;

public class MockUserIdentity : IUserIdentity
{
    public string Id { get; set; } = "xPTL3ja8qr";
    public string Provider { get; set; } = "basic";
}
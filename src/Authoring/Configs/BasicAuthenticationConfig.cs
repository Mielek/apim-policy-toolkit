// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.ApiManagement.PolicyToolkit.Authoring;

public record BasicAuthenticationConfig : IAuthenticationConfig
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}
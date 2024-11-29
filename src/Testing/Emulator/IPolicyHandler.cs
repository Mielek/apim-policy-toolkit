// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator;

internal interface IPolicyHandler
{
    public string PolicyName { get; }
    object? Handle(GatewayContext context, object?[]? args);
}
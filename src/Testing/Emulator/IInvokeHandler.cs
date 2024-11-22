// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator;

public interface IInvokeHandler
{
    public string MethodName { get; }
    object? Invoke(GatewayContext context, object?[]? args);
}
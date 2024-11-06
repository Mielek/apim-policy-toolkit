// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Azure.ApiManagement.PolicyToolkit.Emulator.Expressions;

public class MockContextApi : MockApi, IContextApi
{
    public bool IsCurrentRevision { get; set; } = true;

    public string Revision { get; set; } = "2";

    public string Version { get; set; } = "v2";
}
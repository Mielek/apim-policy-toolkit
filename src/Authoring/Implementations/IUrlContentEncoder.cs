// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.ApiManagement.PolicyToolkit.Authoring.Implementations;

public interface IUrlContentEncoder
{
    string? Encode(IDictionary<string, IList<string>>? dictionary);
}
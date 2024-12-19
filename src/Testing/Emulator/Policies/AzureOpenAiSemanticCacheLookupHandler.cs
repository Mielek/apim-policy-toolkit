// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[Section(nameof(IInboundContext))]
internal class AzureOpenAiSemanticCacheLookupHandler : LlmSemanticCacheLookupHandler
{
    public override string PolicyName => nameof(IInboundContext.AzureOpenAiSemanticCacheLookup);
}
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[Section(nameof(IOutboundContext))]
internal class AzureOpenAiSemanticCacheStoreHandler : LlmSemanticCacheStoreHandler
{
    public override string PolicyName => nameof(IOutboundContext.AzureOpenAiSemanticCacheStore);
}
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[Section(nameof(IInboundContext))]
internal class AzureOpenAiEmitTokenMetricHandler : LlmEmitTokenMetricHandler
{
    public override string PolicyName => nameof(IInboundContext.AzureOpenAiEmitTokenMetric);
}
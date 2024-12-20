// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[Section(nameof(IInboundContext))]
internal class LlmEmitTokenMetricHandler : PolicyHandler<EmitTokenMetricConfig>
{
    public override string PolicyName => nameof(IInboundContext.LlmEmitTokenMetric);

    protected override void Handle(GatewayContext context, EmitTokenMetricConfig config)
    {
        throw new NotImplementedException();
    }
}
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[Section(nameof(IInboundContext))]
internal class CorsHandler : PolicyHandler<CorsConfig>
{
    public override string PolicyName => nameof(IInboundContext.Cors);

    protected override void Handle(GatewayContext context, CorsConfig config)
    {
        throw new NotImplementedException();
    }
}
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[Section(nameof(IInboundContext))]
internal class CacheRemoveValueHandler : PolicyHandler<CacheRemoveValueConfig>
{
    public override string PolicyName => nameof(IInboundContext.CacheRemoveValue);

    protected override void Handle(GatewayContext context, CacheRemoveValueConfig config)
    {
        throw new NotImplementedException();
    }
}
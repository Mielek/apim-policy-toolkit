// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[
    Section(nameof(IInboundContext)),
    Section(nameof(IBackendContext)),
    Section(nameof(IOutboundContext)),
    Section(nameof(IOnErrorContext))
]
internal class CacheStoreValueHandler : PolicyHandler<CacheStoreValueConfig>
{
    public override string PolicyName => nameof(IInboundContext.CacheStoreValue);

    protected override void Handle(GatewayContext context, CacheStoreValueConfig config)
    {
        throw new NotImplementedException();
    }
}
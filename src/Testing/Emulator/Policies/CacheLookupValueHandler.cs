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
internal class CacheLookupValueHandler : PolicyHandler<CacheLookupValueConfig>
{
    public override string PolicyName => nameof(IInboundContext.CacheLookupValue);

    protected override void Handle(GatewayContext context, CacheLookupValueConfig config)
    {
        throw new NotImplementedException();
    }
}
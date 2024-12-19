// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[Section(nameof(IInboundContext))]
internal class IpFilterHandler : PolicyHandler<IpFilterConfig>
{
    public override string PolicyName => nameof(IInboundContext.IpFilter);

    protected override void Handle(GatewayContext context, IpFilterConfig config)
    {
        throw new NotImplementedException();
    }
}
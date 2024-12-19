// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[Section(nameof(IInboundContext))]
internal class CheckHeaderHandler : PolicyHandler<CheckHeaderConfig>
{
    public override string PolicyName => nameof(IInboundContext.CheckHeader);

    protected override void Handle(GatewayContext context, CheckHeaderConfig config)
    {
        throw new NotImplementedException();
    }
}
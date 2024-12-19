// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[Section(nameof(IInboundContext))]
internal class RemoveQueryParameterHandler : PolicyHandler<string>
{
    public override string PolicyName => nameof(IInboundContext.RemoveQueryParameter);
    protected override void Handle(GatewayContext context, string name) => context.Request.Url.Query.Remove(name);
}
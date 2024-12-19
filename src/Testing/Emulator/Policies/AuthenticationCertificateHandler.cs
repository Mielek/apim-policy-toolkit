// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[Section(nameof(IInboundContext))]
internal class AuthenticationCertificateHandler : PolicyHandler<CertificateAuthenticationConfig>
{
    public override string PolicyName => nameof(IInboundContext.AuthenticationCertificate);

    protected override void Handle(GatewayContext context, CertificateAuthenticationConfig config)
    {
        throw new NotImplementedException();
    }
}
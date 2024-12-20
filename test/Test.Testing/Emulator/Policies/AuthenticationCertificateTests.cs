// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing;
using Azure.ApiManagement.PolicyToolkit.Testing.Document;

namespace Test.Emulator.Emulator.Policies;

[TestClass]
public class AuthenticationCertificateTests
{
    class SimpleAuthenticationCertificate : IDocument
    {
        public void Inbound(IInboundContext context)
        {
            context.AuthenticationCertificate(new CertificateAuthenticationConfig { CertificateId = "abcdefgh" });
        }
    }

    [TestMethod]
    public void AuthenticationCertificate_Callback()
    {
        var test = new SimpleAuthenticationCertificate().AsTestDocument();
        var executedCallback = false;
        test.SetupInbound().AuthenticationCertificate().WithCallback((_, _) =>
        {
            executedCallback = true;
        });

        test.RunInbound();

        executedCallback.Should().BeTrue();
    }
}
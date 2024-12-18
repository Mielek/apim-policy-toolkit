// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IdentityModel.Tokens.Jwt;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;
using Azure.ApiManagement.PolicyToolkit.Testing;
using Azure.ApiManagement.PolicyToolkit.Testing.Document;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator;

using Microsoft.IdentityModel.Tokens;

namespace Test.Emulator.Emulator.Policies;

[TestClass]
public class AuthenticationManagedIdentityHandlerTests
{
    class SimpleAmi : IDocument
    {
        public void Inbound(IInboundContext context)
        {
            context.AuthenticationManagedIdentity(new ManagedIdentityAuthenticationConfig()
            {
                Resource = "https://management.azure.com/"
            });
        }
    }

    class AmiClientId : IDocument
    {
        public void Inbound(IInboundContext context)
        {
            context.AuthenticationManagedIdentity(new ManagedIdentityAuthenticationConfig()
            {
                Resource = "https://management.azure.com/", ClientId = "some-client-id"
            });
        }
    }

    class AmiOutputVariable : IDocument
    {
        public void Inbound(IInboundContext context)
        {
            context.AuthenticationManagedIdentity(new ManagedIdentityAuthenticationConfig()
            {
                Resource = "https://management.azure.com/", OutputTokenVariableName = "testVariable"
            });
        }
    }

    class AmiIgnoreError : IDocument
    {
        public void Inbound(IInboundContext context)
        {
            context.AuthenticationManagedIdentity(new ManagedIdentityAuthenticationConfig()
            {
                Resource = "https://management.azure.com/", IgnoreError = true, OutputTokenVariableName = "testVariable"
            });
        }
    }

    class PredicateAmi : IDocument
    {
        public void Inbound(IInboundContext context)
        {
            context.AuthenticationManagedIdentity(new ManagedIdentityAuthenticationConfig()
            {
                Resource = "a", OutputTokenVariableName = "a"
            });
            context.AuthenticationManagedIdentity(new ManagedIdentityAuthenticationConfig()
            {
                Resource = "b", OutputTokenVariableName = "b"
            });
        }
    }

    [TestMethod]
    public void AuthenticationManagedIdentity_HandleSimpleConfig()
    {
        var test = new TestDocument(new SimpleAmi());

        test.RunInbound();

        var authHeader = test.Context.Request.Headers.GetValueOrDefault("Authorization");
        authHeader.Should().NotBeNullOrEmpty().And.StartWithEquivalentOf("Bearer ");
        var token = new JwtSecurityTokenHandler().ReadJwtToken(authHeader!["Bearer ".Length..]);
        token.Issuer.Should().Be("system-assigned");
        token.Audiences.Should().ContainSingle().Which.Should().Be("https://management.azure.com/");
        token.SignatureAlgorithm.Should().Be(SecurityAlgorithms.HmacSha256);
    }

    [TestMethod]
    public void AuthenticationManagedIdentity_HandleClientId()
    {
        var test = new TestDocument(new AmiClientId());

        test.RunInbound();

        var authHeader = test.Context.Request.Headers.GetValueOrDefault("Authorization");
        authHeader.Should().NotBeNullOrEmpty().And.StartWithEquivalentOf("Bearer ");
        var token = new JwtSecurityTokenHandler().ReadJwtToken(authHeader!["Bearer ".Length..]);
        token.Issuer.Should().Be("some-client-id");
        token.Audiences.Should().ContainSingle().Which.Should().Be("https://management.azure.com/");
        token.SignatureAlgorithm.Should().Be(SecurityAlgorithms.HmacSha256);
    }


    [TestMethod]
    public void AuthenticationManagedIdentity_HandleOutputVariable()
    {
        var test = new TestDocument(new AmiOutputVariable());

        test.RunInbound();

        test.Context.Request.Headers.Should().NotContainKey("Authorization");
        var value = test.Context.Variables.GetValueOrDefault("testVariable");
        value.Should().NotBeNull().And.BeOfType<string>().Which.Should().NotBeNullOrEmpty();
        var token = new JwtSecurityTokenHandler().ReadJwtToken(value as string);
        token.Issuer.Should().Be("system-assigned");
        token.Audiences.Should().ContainSingle().Which.Should().Be("https://management.azure.com/");
        token.SignatureAlgorithm.Should().Be(SecurityAlgorithms.HmacSha256);
    }

    [TestMethod]
    public void AuthenticationManagedIdentity_HandleCallback()
    {
        var test = new TestDocument(new SimpleAmi());
        ManagedIdentityAuthenticationConfig? config = null;
        test.InInbound().AuthenticationManagedIdentity().WithCallback((context, cfg) =>
        {
            context.Variables["test"] = "test";
            config = cfg;
        });

        test.RunInbound();

        test.Context.Request.Headers.Should().NotContainKey("Authorization");
        var variable = test.Context.Variables.Should().ContainSingle().Which;
        variable.Key.Should().Be("test");
        variable.Value.Should().BeOfType<string>().And.Be("test");
        config.Should().NotBeNull();
        config!.Resource.Should().Be("https://management.azure.com/");
    }

    [TestMethod]
    public void AuthenticationManagedIdentity_HandleCallback_WithInvocationPredicate()
    {
        var test = new TestDocument(new PredicateAmi());
        test.InInbound()
            .AuthenticationManagedIdentity((_, config) => config.Resource == "c")
            .WithCallback((context, _) => context.Variables["c"] = "token-c");
        test.InInbound()
            .AuthenticationManagedIdentity((_, config) => config.Resource == "b")
            .WithCallback((context, _) => context.Variables["b"] = "token-b");
        test.InInbound()
            .AuthenticationManagedIdentity((_, config) => config.Resource == "a")
            .WithCallback((context, _) => context.Variables["a"] = "token-a");

        test.RunInbound();

        test.Context.Request.Headers.Should().NotContainKey("Authorization");
        test.Context.Variables.Should().ContainKeys("a", "b");
    }

    [TestMethod]
    public void AuthenticationManagedIdentity_HandleTokenProviderHook()
    {
        var test = new TestDocument(new AmiClientId());
        test.InInbound()
            .AuthenticationManagedIdentity()
            .WithTokenProviderHook((resource, clientId) => $"{resource}{clientId}/token");

        test.RunInbound();

        test.Context.Request.Headers.Should().ContainKey("Authorization")
            .WhoseValue.Should().ContainSingle()
            .Which.Should().Be("Bearer https://management.azure.com/some-client-id/token");
    }

    [TestMethod]
    public void AuthenticationManagedIdentity_HandleTokenProviderHook_WithInvocationPredicate()
    {
        var test = new TestDocument(new PredicateAmi());
        test.InInbound()
            .AuthenticationManagedIdentity((_, config) => config.Resource == "c")
            .WithTokenProviderHook((_, _) => "token-c");
        test.InInbound()
            .AuthenticationManagedIdentity((_, config) => config.Resource == "b")
            .WithTokenProviderHook((_, _) => "token-b");
        test.InInbound()
            .AuthenticationManagedIdentity((_, config) => config.Resource == "a")
            .WithTokenProviderHook((_, _) => "token-a");

        test.RunInbound();

        test.Context.Request.Headers.Should().NotContainKey("Authorization");
        test.Context.Variables.Should().Contain("a", "token-a").And.Contain("b", "token-b");
    }

    [TestMethod]
    public void AuthenticationManagedIdentity_HandleReturnToken()
    {
        var test = new TestDocument(new SimpleAmi());
        test.InInbound().AuthenticationManagedIdentity().ReturnsToken("token");

        test.RunInbound();

        test.Context.Request.Headers.Should().ContainKey("Authorization")
            .WhoseValue.Should().ContainSingle()
            .Which.Should().Be("Bearer token");
    }

    [TestMethod]
    public void AuthenticationManagedIdentity_HandleReturnToken_WithInvocationPredicate()
    {
        var test = new TestDocument(new PredicateAmi());
        test.InInbound()
            .AuthenticationManagedIdentity((_, config) => config.Resource == "c")
            .ReturnsToken("token-c");
        test.InInbound()
            .AuthenticationManagedIdentity((_, config) => config.Resource == "b")
            .ReturnsToken("token-b");
        test.InInbound()
            .AuthenticationManagedIdentity((_, config) => config.Resource == "a")
            .ReturnsToken("token-a");

        test.RunInbound();

        test.Context.Request.Headers.Should().NotContainKey("Authorization");
        test.Context.Variables.Should().Contain("a", "token-a").And.Contain("b", "token-b");
    }

    [TestMethod]
    public void AuthenticationManagedIdentity_HandleWithError()
    {
        var test = new TestDocument(new SimpleAmi());
        test.InInbound().AuthenticationManagedIdentity().WithError("InternalServerError");

        var ex = Assert.ThrowsException<PolicyException>(() => test.RunInbound());

        ex.Policy.Should().Be("AuthenticationManagedIdentity");
        ex.Section.Should().Be("IInboundContext");
        ex.Message.Should().Be("InternalServerError");
        var data = ex.PolicyArgs.Should().NotBeNull().And.ContainSingle()
            .Which.Should().BeOfType<ManagedIdentityAuthenticationConfig>().Which;
        data.Resource.Should().Be("https://management.azure.com/");
        ex.InnerException.Should().NotBeNull().And.BeOfType<HttpRequestException>().Which.Message.Should()
            .Be("InternalServerError");
        test.Context.Request.Headers.Should().NotContainKey("Authorization");
        test.Context.Variables.Should().BeEmpty();
    }

    [TestMethod]
    public void AuthenticationManagedIdentity_HandleWithError_WithInvocationPredicate()
    {
        var test = new TestDocument(new PredicateAmi());
        test.InInbound()
            .AuthenticationManagedIdentity((_, config) => config.Resource == "b")
            .WithError("InternalServerError");
        test.InInbound()
            .AuthenticationManagedIdentity((_, config) => config.Resource == "a")
            .ReturnsToken("token-a");

        var ex = Assert.ThrowsException<PolicyException>(() => test.RunInbound());
        ex.Policy.Should().Be("AuthenticationManagedIdentity");
        ex.Section.Should().Be("IInboundContext");
        ex.Message.Should().Be("InternalServerError");
        var config = ex.PolicyArgs.Should().NotBeNull().And.ContainSingle()
            .Which.Should().BeOfType<ManagedIdentityAuthenticationConfig>().Which;
        config.Resource.Should().Be("b");
        ex.InnerException.Should().NotBeNull().And.BeOfType<HttpRequestException>().Which.Message.Should()
            .Be("InternalServerError");
        test.Context.Request.Headers.Should().NotContainKey("Authorization");
        var variable = test.Context.Variables.Should().ContainSingle().Which;
        variable.Key.Should().Be("a");
        variable.Value.Should().Be("token-a");
    }
    
    [TestMethod]
    public void AuthenticationManagedIdentity_IgnoreError()
    {
        var test = new TestDocument(new AmiIgnoreError());
        test.InInbound().AuthenticationManagedIdentity().WithError("InternalServerError");

        test.RunInbound();

        var authHeader = test.Context.Request.Headers.GetValueOrDefault("Authorization");
        authHeader.Should().NotBeNullOrEmpty().And.StartWithEquivalentOf("Bearer ");
        authHeader!["Bearer ".Length..].Should().BeEmpty();
    }
}
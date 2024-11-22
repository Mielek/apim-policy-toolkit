// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Handlers;

using Newtonsoft.Json.Linq;

namespace Azure.ApiManagement.PolicyToolkit.Testing;

[TestClass]
public class Example
{
    [TestMethod]
    public void ShouldUseBasicAuthenticationForRequestsFromInternalIp()
    {
        var context = new GatewayContext { RuntimeContext = { Request = { IpAddress = "10.0.0.1" } } };

        new OperationDocument().Inbound(context.InboundContext);

        var authValue = context.RuntimeContext.Request
            .Headers.Should().ContainKey("Authorization")
            .WhoseValue.Should().HaveCount(1).And.Subject.First()!;
        authValue.Should().StartWith("Basic ");
        var token = authValue.Substring("Basic ".Length);
        token = Encoding.UTF8.GetString(Convert.FromBase64String(token));
        token.Should().Be("{{username}}:{{password}}");
    }

    [TestMethod]
    public void ShouldUseManagedIdentityTokenAuthenticationForRequestsFromExternalIp()
    {
        var context = new GatewayContext { RuntimeContext = { Request = { IpAddress = "11.0.0.1" } } };
        bool baseInvoked = false;
        context.SetHandler<IInboundContext>(new BaseHandler() { Interceptor = _ => baseInvoked = true });

        new OperationDocument().Inbound(context.InboundContext);

        baseInvoked.Should().BeTrue();
        var authValue = context.RuntimeContext.Request
            .Headers.Should().ContainKey("Authorization")
            .WhoseValue.Should().HaveCount(1).And.Subject.First()!;
        authValue.Should().StartWith("Bearer ");
        var token = authValue.Substring("Bearer ".Length);
        var variableToken = context.RuntimeContext
            .Variables.Should().ContainKey("testToken")
            .WhoseValue.Should().BeOfType<string>().Subject;
        token.Should().Be(variableToken).And.Be("resource=https://management.azure.com/");
    }

    [TestMethod]
    public void ShouldRewriteBody()
    {
        var initial = JObject.Parse(
            """
            {
                "title": "Software Engineer",
                "location": "Redmond",
                "secret": "42",
                "name": "John Doe"
            }
            """);
        var context = new GatewayContext
        {
            RuntimeContext = { Response = { Body = { Content = initial.ToString() } } }
        };

        new OperationDocument().Outbound(context.OutboundContext);

        var body = context.RuntimeContext.Response.Body.Content;
        var expected = JObject.Parse(
            """
            {
                "title": "Software Engineer",
                "name": "John Doe"
            }
            """);
        Assert.IsTrue(JToken.DeepEquals(
            JObject.Parse(body),
            expected
        ));
    }

    [TestMethod]
    public void Emulator()
    {
        var emulator = new GatewayEmulator()
        {
            Documents =
            {
                { DocumentScope.Global, new GlobalDocument() },
                { DocumentScope.Api, new BlankDocument() },
                { DocumentScope.Operation, new OperationDocument() }
            }
        };
        emulator.Context.SetHandler<IBackendContext>(new ForwardRequestHandler() { Interceptor = (_, cfg) => Assert.IsNull(cfg)});

        emulator.Run();
        
        
    }


    class BlankDocument : IDocument
    {
        public void Inbound(IInboundContext context)
        {
            context.Base();
        }

        public void Backend(IBackendContext context)
        {
            context.Base();
        }

        public void Outbound(IOutboundContext context)
        {
            context.Base();
        }
    }

    class GlobalDocument : IDocument
    {
        public void Backend(IBackendContext context)
        {
            context.ForwardRequest();
        }
    }

    class OperationDocument : IDocument
    {
        public void Inbound(IInboundContext context)
        {
            context.Base();
            if (IsFromCompanyIp(context.ExpressionContext))
            {
                context.AuthenticationBasic("{{username}}", "{{password}}");
            }
            else
            {
                context.AuthenticationManagedIdentity(new ManagedIdentityAuthenticationConfig()
                {
                    Resource = "https://management.azure.com/", OutputTokenVariableName = "testToken",
                });
                context.SetHeader("Authorization", Bearer(context.ExpressionContext));
            }
        }

        public void Backend(IBackendContext context)
        {
            context.Base();
        }

        public void Outbound(IOutboundContext context)
        {
            context.Base();
            context.SetBody(FilterSecrets(context.ExpressionContext));
        }

        public bool IsFromCompanyIp(IExpressionContext context)
            => context.Request.IpAddress.StartsWith("10.0.0.");

        public string Bearer(IExpressionContext context)
            => $"Bearer {context.Variables["testToken"]}";

        [Expression]
        public string FilterSecrets(IExpressionContext context)
        {
            var body = context.Response.Body.As<JObject>();
            foreach (var internalProperty in new string[] { "location", "secret" })
            {
                if (body.ContainsKey(internalProperty))
                {
                    body.Remove(internalProperty);
                }
            }

            return body.ToString();
        }
    }
}
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.ApiManagement.PolicyToolkit.Compilation;

[TestClass]
public class CheckHeaderTests
{
    [TestMethod]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.CheckHeader(new CheckHeaderConfig
                {
                    Name = "name",
                    FailCheckHttpCode = 400,
                    FailCheckErrorMessage = "message",
                    IgnoreCase = "false",
                    Values = ["value1", "value2"]
                });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <check-header name="name" failed-check-httpcode="400" failed-check-error-message="message" ignore-case="false">
                    <value>value1</value>
                    <value>value2</value>
                </check-header>
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile check-header policy"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.CheckHeader(new CheckHeaderConfig
                {
                    Name = NameExp(context.ExpressionContext),
                    FailCheckHttpCode = 400,
                    FailCheckErrorMessage = "message",
                    IgnoreCase = false,
                    Values = ["value1", "value2"]
                });
            }
            string NameExp(IExpressionContext context) => context.Product.Name;
        }
        """,
        """
        <policies>
            <inbound>
                <check-header name="@(context.Product.Name)" failed-check-httpcode="400" failed-check-error-message="message" ignore-case="false">
                    <value>value1</value>
                    <value>value2</value>
                </check-header>
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile check-header policy with expression in name"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.CheckHeader(new CheckHeaderConfig
                {
                    Name = "name",
                    FailCheckHttpCode = CodeExp(context.ExpressionContext),
                    FailCheckErrorMessage = "message",
                    IgnoreCase = false,
                    Values = ["value1", "value2"]
                });
            }
            int CodeExp(IExpressionContext context) => context.Product.Name == "name" ? 400 : 500;
        }
        """,
        """
        <policies>
            <inbound>
                <check-header name="name" failed-check-httpcode="@(context.Product.Name == "name" ? 400 : 500)" failed-check-error-message="message" ignore-case="false">
                    <value>value1</value>
                    <value>value2</value>
                </check-header>
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile check-header policy with expressions in FailCheckHttpCode"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.CheckHeader(new CheckHeaderConfig
                {
                    Name = "name",
                    FailCheckHttpCode = 400,
                    FailCheckErrorMessage = MessageExp(context.ExpressionContext),
                    IgnoreCase = false,
                    Values = ["value1", "value2"]
                });
            }
            string MessageExp(IExpressionContext context) => $"Error: {context.Product.Name}";
        }
        """,
        """
        <policies>
            <inbound>
                <check-header name="name" failed-check-httpcode="400" failed-check-error-message="@($"Error: {context.Product.Name}")" ignore-case="false">
                    <value>value1</value>
                    <value>value2</value>
                </check-header>
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile check-header policy with expression in FailCheckErrorMessage"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.CheckHeader(new CheckHeaderConfig
                {
                    Name = "name",
                    FailCheckHttpCode = 400,
                    FailCheckErrorMessage = "message",
                    IgnoreCase = IgnoreCaseExp(context.ExpressionContext),
                    Values = ["value1", "value2"]
                });
            }
            bool IgnoreCaseExp(IExpressionContext context) => context.Product.Name == "name";
        }
        """,
        """
        <policies>
            <inbound>
                <check-header name="name" failed-check-httpcode="400" failed-check-error-message="message" ignore-case="@(context.Product.Name == "name")">
                    <value>value1</value>
                    <value>value2</value>
                </check-header>
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile check-header policy with expression in IgnoreCase"
    )]
    public void ShouldCompileCheckHeaderPolicy(string code, string expectedXml)
    {
        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }
}
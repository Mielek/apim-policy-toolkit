using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;
using Mielek.Azure.ApiManagement.PolicyToolkit.CodeContext;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

public class CSharpPolicyCompiler
{
    private readonly ClassDeclarationSyntax document;

    public CSharpPolicyCompiler(ClassDeclarationSyntax document)
    {
        this.document = document;
    }

    public XElement Compile()
    {
        var methods = document.DescendantNodes()
            .OfType<MethodDeclarationSyntax>();
        var policyDocument = new XElement("policies");

        foreach (var method in methods)
        {
            switch (method.Identifier.ValueText)
            {
                case nameof(ICodeDocument.Inbound):
                    var inbound = CompileSection("inbound", method.Body);
                    policyDocument.Add(inbound);
                    break;
                case nameof(ICodeDocument.Outbound):
                    var outbound = CompileSection("outbound", method.Body);
                    policyDocument.Add(outbound);
                    break;

            }
        }

        return policyDocument;
    }


    private XElement CompileSection(string section, BlockSyntax block)
    {
        var sectionElement = new XElement(section);
        foreach (var statement in block.Statements)
        {
            switch (statement)
            {
                case LocalDeclarationStatementSyntax syntax:
                    ProcessLocalDeclaration(syntax, sectionElement);
                    break;
                case ExpressionStatementSyntax syntax:
                    ProcessExpression(syntax, sectionElement);
                    break;
                case IfStatementSyntax syntax:
                    ProcessIf(syntax, sectionElement);
                    break;
            }
        }

        return sectionElement;
    }

    private void ProcessIf(IfStatementSyntax syntax, XElement sectionElement)
    {
        var choose = new XElement("choose");
        sectionElement.Add(choose);

        var whenSection = CompileSection("when", syntax.Statement as BlockSyntax);
        choose.Add(whenSection);

        whenSection.Add(new XAttribute("condition", FindCode(syntax.Condition as InvocationExpressionSyntax)));

        if (syntax.Else != null)
        {
            var otherwiseSection = CompileSection("otherwise", syntax.Else.Statement as BlockSyntax);
            choose.Add(otherwiseSection);
        }
    }

    private void ProcessExpression(ExpressionStatementSyntax syntax, XElement section)
    {
        var invocation = syntax.Expression as InvocationExpressionSyntax;
        var memberAccess = invocation.Expression as MemberAccessExpressionSyntax;
        switch (memberAccess.Name.ToString())
        {
            case "SetHeader":
                ProcessSetHeader(section, invocation);
                break;
            case "RemoveHeader":
                ProcessRemoveHeader(section, invocation);
                break;
            case "SetBody":
                ProcessSetBody(section, invocation);
                break;
            case "Base":
                section.Add(new XElement("base"));
                break;
            case "AuthenticationBasic":
                ProcessAuthenticationBasic(section, invocation);
                break;
            case "AuthenticationManagedIdentity":
                var resource = ProcessParameter(invocation.ArgumentList.Arguments[0].Expression);
                section.Add(new AuthenticationManagedIdentityPolicyBuilder()
                    .Resource(resource)
                    .Build());
                break;
        }
    }

    private void ProcessAuthenticationBasic(XElement section, InvocationExpressionSyntax invocation)
    {
        var username = ProcessParameter(invocation.ArgumentList.Arguments[0].Expression);
        var password = ProcessParameter(invocation.ArgumentList.Arguments[1].Expression);
        section.Add(new AuthenticationBasicPolicyBuilder()
            .Username(username)
            .Password(password)
            .Build());
    }

    private void ProcessSetBody(XElement section, InvocationExpressionSyntax invocation)
    {
        var value = ProcessParameter(invocation.ArgumentList.Arguments[0].Expression);
        section.Add(new SetBodyPolicyBuilder()
            .Body(value)
            .Build());
    }

    private void ProcessRemoveHeader(XElement section, InvocationExpressionSyntax invocation)
    {
        var headerName = ProcessParameter(invocation.ArgumentList.Arguments[0].Expression);
        section.Add(new SetHeaderPolicyBuilder()
            .Name(headerName)
            .ExistsAction(SetHeaderPolicyBuilder.ExistsActionType.Delete)
            .Build());
    }

    private void ProcessSetHeader(XElement section, InvocationExpressionSyntax invocation)
    {
        var headerName = ProcessParameter(invocation.ArgumentList.Arguments[0].Expression);
        var headerValue = ProcessParameter(invocation.ArgumentList.Arguments[1].Expression);
        section.Add(new SetHeaderPolicyBuilder()
            .Name(headerName)
            .Value(headerValue)
            .Build());
    }

    private void ProcessLocalDeclaration(LocalDeclarationStatementSyntax syntax, XElement section)
    {
        var variable = syntax.Declaration.Variables[0];
        var invocation = variable.Initializer.Value as InvocationExpressionSyntax;
        var memberAccess = invocation.Expression as MemberAccessExpressionSyntax;
        switch (memberAccess.Name.ToString())
        {
            case "AuthenticationManagedIdentity":
                var resource = ProcessParameter(invocation.ArgumentList.Arguments[0].Expression);
                section.Add(new AuthenticationManagedIdentityPolicyBuilder()
                    .Resource(resource)
                    .OutputTokenVariableName(variable.Identifier.ValueText)
                    .Build());
                break;
        }

    }

    private string ProcessParameter(ExpressionSyntax expression)
    {
        switch (expression)
        {
            case LiteralExpressionSyntax syntax:
                return syntax.Token.ValueText;
            case InterpolatedStringExpressionSyntax syntax:
                var interpolationParts = syntax.Contents.Select(c => c switch
                {
                    InterpolatedStringTextSyntax text => text.TextToken.ValueText,
                    InterpolationSyntax interpolation =>
                        $"{{context.Variables[\"{interpolation.Expression.ToString()}\"]}}",
                    _ => ""
                });
                return new LambdaExpression<string>($"context => $\"{string.Join("", interpolationParts)}\"").Source;
            case AnonymousFunctionExpressionSyntax syntax:
                return new LambdaExpression<string>(syntax.ToString()).Source;
            case InvocationExpressionSyntax syntax:
                return FindCode(syntax);
        }

        return "";
    }

    private string FindCode(InvocationExpressionSyntax syntax)
    {
        var methodIdentifier = (syntax.Expression as IdentifierNameSyntax).Identifier.ValueText;
        var expressionMethod = document.DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .First(m => m.Identifier.ValueText == methodIdentifier);

        if (expressionMethod.Body != null)
        {
            return new LambdaExpression<bool>($"context => {expressionMethod.Body}").Source;
        }
        else if (expressionMethod.ExpressionBody != null)
        {
            return new LambdaExpression<bool>($"context => {expressionMethod.ExpressionBody.Expression}").Source;
        }
        else
        {
            throw new InvalidOperationException("Invalid expression");
        }
    }
}
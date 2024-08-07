using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;
using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;
using Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Policy;
using Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Syntax;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

public class CSharpPolicyCompiler
{
    private ClassDeclarationSyntax _document;

    private BlockCompiler _blockCompiler;

    public CSharpPolicyCompiler(ClassDeclarationSyntax document)
    {
        _document = document;
        var invStatement = new ExpressionStatementCompiler([
            new BaseCompiler(),
            SetHeaderCompiler.AppendCompiler,
            SetHeaderCompiler.SetCompiler,
            SetHeaderCompiler.SetIfNotExistCompiler,
            SetHeaderCompiler.RemoveCompiler,
            new SetBodyCompiler(),
            new AuthenticationBasicCompiler()
        ]);
        var loc = new LocalDeclarationStatementCompiler([
            new AuthenticationManageIdentityCompiler()
        ]);
        _blockCompiler = new([
            invStatement,
            loc
        ]);
        _blockCompiler.AddCompiler(new IfStatementCompiler(_blockCompiler));
    }

    public ICompilationResult Compile()
    {
        var methods = _document.DescendantNodes()
            .OfType<MethodDeclarationSyntax>();
        var policyDocument = new XElement("policies");
        var context = new CompilationContext(_document, policyDocument);

        foreach (var method in methods)
        {
            var sectionName = method.Identifier.ValueText switch
            {
                nameof(IDocument.Inbound) => "inbound",
                nameof(IDocument.Outbound) => "outbound",
                nameof(IDocument.Backend) => "backend",
                nameof(IDocument.OnError) => "on-error",
                _ => string.Empty
            };

            if (string.IsNullOrEmpty(sectionName))
            {
                continue;
            }

            if (method.Body is null)
            {
                context.ReportError($"Method {sectionName} is not allowed as expression. ({method.GetLocation()})");
                continue;
            }

            var section = CompileSection(context, sectionName, method.Body);
            context.AddPolicy(section);
        }

        return context;
    }


    private XElement CompileSection(ICompilationContext context, string section, BlockSyntax block)
    {
        var sectionElement = new XElement(section);
        var sectionContext = new SubCompilationContext(context, sectionElement);
        _blockCompiler.Compile(sectionContext, block);
        // foreach (var statement in block.Statements)
        // {
        //     switch (statement)
        //     {
        //         case LocalDeclarationStatementSyntax syntax:
        //             ProcessLocalDeclaration(syntax, sectionElement);
        //             break;
        //         case ExpressionStatementSyntax syntax:
        //             ProcessExpression(syntax, sectionElement);
        //             break;
        //         case IfStatementSyntax syntax:
        //             ProcessIf(syntax, sectionElement);
        //             break;
        //     }
        // }

        return sectionElement;
    }

    // private void ProcessIf(IfStatementSyntax syntax, XElement sectionElement)
    // {
    //     var choose = new XElement("choose");
    //     sectionElement.Add(choose);

    //     IfStatementSyntax? nextIf = syntax;
    //     IfStatementSyntax currentIf;
    //     do
    //     {
    //         currentIf = nextIf;
    //         var whenSection = CompileSection("when", currentIf.Statement as BlockSyntax);
    //         choose.Add(whenSection);

    //         whenSection.Add(new XAttribute("condition", FindCode(currentIf.Condition as InvocationExpressionSyntax)));

    //         nextIf = currentIf.Else?.Statement as IfStatementSyntax;
    //     } while (nextIf != null);


    //     if (currentIf.Else != null)
    //     {
    //         var otherwiseSection = CompileSection("otherwise", currentIf.Else.Statement as BlockSyntax);
    //         choose.Add(otherwiseSection);
    //     }
    // }

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
            .ExistsAction(SetHeaderPolicyBuilder.ExistsActionType.Override)
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
        var expressionMethod = _document.DescendantNodes()
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
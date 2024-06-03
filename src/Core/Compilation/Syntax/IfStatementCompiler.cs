using System.Net;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Syntax;

public class IfStatementCompiler : ISyntaxCompiler
{
    ISyntaxCompiler _blockCompiler;

    public IfStatementCompiler(ISyntaxCompiler blockCompiler)
    {
        this._blockCompiler = blockCompiler;
    }

    public SyntaxKind Syntax => SyntaxKind.IfStatement;

    public void Compile(ICompilationContext context, SyntaxNode node)
    {
        var ifStatement = node as IfStatementSyntax ?? throw new NullReferenceException();

        var choose = new XElement("choose");
        context.AddPolicy(choose);

        IfStatementSyntax? nextIf = ifStatement;
        IfStatementSyntax currentIf;
        do
        {
            currentIf = nextIf;
            var section = new XElement("when");
            var innerContext = new SubCompilationContext(context, section);
            _blockCompiler.Compile(innerContext, currentIf.Statement as BlockSyntax);
            section.Add(new XAttribute("condition", CompilerUtils.FindCode(context, currentIf.Condition as InvocationExpressionSyntax)));
            choose.Add(section);

            nextIf = currentIf.Else?.Statement as IfStatementSyntax;
        } while (nextIf != null);


        if (currentIf.Else != null)
        {
            var section = new XElement("otherwise");
            var innerContext = new SubCompilationContext(context, section);
            _blockCompiler.Compile(innerContext, currentIf.Else.Statement as BlockSyntax);
            choose.Add(section);
        }
    }

}
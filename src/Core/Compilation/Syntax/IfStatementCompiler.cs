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
        IfStatementSyntax? nextIf = ifStatement;
        IfStatementSyntax currentIf;
        do
        {
            currentIf = nextIf;
            var innerContext = new IfBlockCompilationContext(context, "when");
            _blockCompiler.Compile(innerContext, currentIf.Statement as BlockSyntax);
            var section = innerContext.SectionElement;
            section.Add(new XAttribute("condition",
                CompilerUtils.FindCode(context, currentIf.Condition as InvocationExpressionSyntax)));
            choose.Add(section);

            nextIf = currentIf.Else?.Statement as IfStatementSyntax;
        } while (nextIf != null);


        if (currentIf.Else != null)
        {
            var innerContext = new IfBlockCompilationContext(context, "otherwise");
            _blockCompiler.Compile(innerContext, currentIf.Else.Statement as BlockSyntax);
            choose.Add(innerContext.SectionElement);
        }
    }

    class IfBlockCompilationContext : ICompilationContext
    {
        public XElement SectionElement { get; }

        private ICompilationContext _parent;

        public IfBlockCompilationContext(ICompilationContext parent, string name)
        {
            this._parent = parent;
            this.SectionElement = new XElement(name);
        }

        public void AddPolicy(XElement element) => SectionElement.Add(element);

        public void ReportError(string message) => _parent.ReportError(message);

        public SyntaxNode Root => _parent.Root;
    }
}
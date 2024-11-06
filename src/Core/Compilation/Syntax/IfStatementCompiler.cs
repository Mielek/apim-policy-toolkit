// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compilation.Syntax;

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

            if (currentIf.Statement is not BlockSyntax block)
            {
                context.ReportError(
                    $"{currentIf.Statement.GetType().Name} is not supported. ({currentIf.Statement.GetLocation()})");
                continue;
            }

            if (currentIf.Condition is not InvocationExpressionSyntax condition)
            {
                context.ReportError(
                    $"{currentIf.Condition.GetType().Name} is not supported. ({currentIf.Condition.GetLocation()})");
                continue;
            }

            var section = new XElement("when");
            var innerContext = new SubCompilationContext(context, section);
            _blockCompiler.Compile(innerContext, block);
            section.Add(new XAttribute("condition", CompilerUtils.FindCode(condition, context)));
            choose.Add(section);

            nextIf = currentIf.Else?.Statement as IfStatementSyntax;
        } while (nextIf != null);

        if (currentIf.Else != null)
        {
            var section = new XElement("otherwise");
            var innerContext = new SubCompilationContext(context, section);
            if (currentIf.Else.Statement is BlockSyntax block)
            {
                _blockCompiler.Compile(innerContext, block);
                choose.Add(section);
            }
            else
            {
                context.ReportError(
                    $"{currentIf.Else.Statement.GetType().Name} is not supported. ({currentIf.Else.Statement.GetLocation()})");
            }
        }
    }
}
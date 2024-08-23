using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Compilation.Syntax;

public class BlockCompiler : ISyntaxCompiler
{
    private IDictionary<SyntaxKind, ISyntaxCompiler> _compilers;

    public BlockCompiler(IEnumerable<ISyntaxCompiler> compilers)
    {
        _compilers = compilers.ToDictionary(c => c.Syntax);
    }

    public void AddCompiler(ISyntaxCompiler compiler)
    {
        _compilers.Add(compiler.Syntax, compiler);
    }

    public SyntaxKind Syntax => SyntaxKind.Block;

    public void Compile(ICompilationContext context, SyntaxNode node)
    {
        var block = node as BlockSyntax ?? throw new NullReferenceException();

        foreach (var statement in block.Statements)
        {
            if (_compilers.TryGetValue(statement.Kind(), out var compiler))
            {
                compiler.Compile(context, statement);
            }
            else
            {
                context.ReportError("");
            }
        }
    }
}

using FluentAssertions.Execution;
using FluentAssertions.Primitives;

using Mielek.Azure.ApiManagement.PolicyToolkit.Compilation;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Assertions;

public class CompilationResultAssertion : ObjectAssertions<ICompilationResult, CompilationResultAssertion>
{
    public CompilationResultAssertion(ICompilationResult value) : base(value)
    {
    }

    public AndConstraint<CompilationResultAssertion> BeSuccessful(string because = "", params object[] becauseArgs)
    {
        using var scope = new AssertionScope();
        scope.BecauseOf(because, becauseArgs);
        this.NotBeNull();
        Subject.Errors.Should().BeEmpty();
        Subject.Document.Should().NotBeNull();
        return new AndConstraint<CompilationResultAssertion>(this);
    }

    public AndConstraint<CompilationResultAssertion> DocumentEquivalentTo(string expectedXml, string because = "", params object[] becauseArgs)
    {
        Subject.Document.Should().BeEquivalentTo(expectedXml, because, becauseArgs);
        return new AndConstraint<CompilationResultAssertion>(this);
    }
}
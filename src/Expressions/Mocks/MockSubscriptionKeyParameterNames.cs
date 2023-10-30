using Mielek.Expressions.Context;

namespace Mielek.Testing.Expressions.Mocks;

public class MockSubscriptionKeyParameterNames : ISubscriptionKeyParameterNames
{
    public string Header { get; set; } = "X-Sub-Header";
    public string Query { get; set; } = "subQuery";
}
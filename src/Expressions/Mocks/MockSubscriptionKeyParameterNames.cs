using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.Mocks;

public class MockSubscriptionKeyParameterNames : ISubscriptionKeyParameterNames
{
    public string Header { get; set; } = "X-Sub-Header";
    public string Query { get; set; } = "subQuery";
}
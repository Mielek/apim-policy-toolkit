using Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Azure.ApiManagement.PolicyToolkit.Emulator.Expressions;

public class MockSubscriptionKeyParameterNames : ISubscriptionKeyParameterNames
{
    public string Header { get; set; } = "X-Sub-Header";
    public string Query { get; set; } = "subQuery";
}
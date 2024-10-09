using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Emulator.Expressions;

public class MockOperation : IOperation
{
    public string Id { get; set; } = "operation-id";
    public string Method { get; set; } = "GET";
    public string Name { get; set; } = "fetch-operation";
    public string UrlTemplate { get; set; } = "/v2/mock/op";
}
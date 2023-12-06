using Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Expressions.Context.Mocks;

public class MockContextApi : MockApi, IContextApi
{
    public bool IsCurrentRevision { get; set; } = true;

    public string Revision { get; set; } = "2";

    public string Version { get; set; } = "v2";
}
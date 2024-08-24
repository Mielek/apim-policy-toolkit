using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Emulator.Expressions;

public class MockContextApi : MockApi, IContextApi
{
    public bool IsCurrentRevision { get; set; } = true;

    public string Revision { get; set; } = "2";

    public string Version { get; set; } = "v2";
}
using System.Security.Cryptography.X509Certificates;

using Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Azure.ApiManagement.PolicyToolkit.Emulator.Expressions;

public class MockDeployment : IDeployment
{
    public string GatewayId { get; set; } = "managed";
    public string Region { get; set; } = "eastus";
    public string ServiceId { get; } = "contoso-dev-apim";
    public string ServiceName { get; } = "contoso-dev-apim";

    public Dictionary<string, X509Certificate2> Certificates { get; set; } = new Dictionary<string, X509Certificate2>();
    IReadOnlyDictionary<string, X509Certificate2> IDeployment.Certificates => Certificates;
}
using System.Security.Cryptography.X509Certificates;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

public interface IDeployment
{
    string GatewayId { get; }

    string Region { get; }

    string ServiceId { get; }

    string ServiceName { get; }

    IReadOnlyDictionary<string, X509Certificate2> Certificates { get; }
}
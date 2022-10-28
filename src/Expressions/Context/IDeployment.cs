using System.Security.Cryptography.X509Certificates;

namespace Mielek.Expressions.Context;

public interface IDeployment
{
    string GatewayId { get; }

    string Region { get; }

    string ServiceId { get; }

    string ServiceName { get; }

    IReadOnlyDictionary<string, X509Certificate2> Certificates { get; }
}
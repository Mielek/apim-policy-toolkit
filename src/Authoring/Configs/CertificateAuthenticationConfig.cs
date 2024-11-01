namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public record CertificateAuthenticationConfig : IAuthenticationConfig
{
    public string? Thumbprint { get; init; }
    public string? CertificateId { get; init; }

    public byte[]? Body { get; init; }
    public string? Password { get; init; }
}
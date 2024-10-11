namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

public record Jwt
{
    public required string Algorithm { get; init; }
    public required IEnumerable<string> Audiences { get; init; }
    public required IReadOnlyDictionary<string, string[]> Claims { get; init; }
    public DateTime? ExpirationTime { get; init; }
    public required string Id { get; init; }
    public required string Issuer { get; init; }
    public DateTime? NotBefore { get; init; }
    public DateTime? IssuedAt { get; init; }
    public required string Subject { get; init; }
    public required string Type { get; init; }
}
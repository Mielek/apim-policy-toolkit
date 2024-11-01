namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public record CacheStoreValueConfig
{
    public required string Key { get; init; }
    public required string Value { get; init; }
    public required uint Duration { get; init; }
    public string? CachingType { get; init; }
}
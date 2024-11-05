namespace Azure.ApiManagement.PolicyToolkit.Authoring;

public record CacheRemoveValueConfig
{
    public required string Key { get; init; }
    public string? CachingType { get; init; }
}
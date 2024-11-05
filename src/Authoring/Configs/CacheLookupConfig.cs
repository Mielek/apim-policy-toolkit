namespace Azure.ApiManagement.PolicyToolkit.Authoring;

public record CacheLookupConfig
{
    public required bool VaryByDeveloper { get; init; }
    public required bool VaryByDeveloperGroups { get; init; }
    public string? CachingType { get; init; }
    public string? DownstreamCachingType { get; init; }
    public bool? MustRevalidate { get; init; }
    public bool? AllowPrivateResponseCaching { get; init; }
    public string[]? VaryByHeaders { get; init; }
    public string[]? VaryByQueryParameters { get; init; }
}
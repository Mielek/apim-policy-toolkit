namespace Azure.ApiManagement.PolicyToolkit.Authoring;

public record SemanticCacheLookupConfig
{
    public required decimal ScoreThreshold { get; init; }
    public required string EmbeddingsBackendId { get; init; }
    public required string EmbeddingsBackendAuth { get; init; }
    public bool? IgnoreSystemMessages { get; init; }
    public uint MaxMessageCount { get; init; }
    public string[]? VaryBy { get; init; }
}
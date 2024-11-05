namespace Azure.ApiManagement.PolicyToolkit.Authoring;

public record SetBackendServiceConfig
{
    public string? BaseUrl { get; init; }
    public string? BackendId { get; init; }
    public bool? SfResolveCondition { get; init; }
    public string? SfServiceInstanceName { get; init; }
    public string? SfPartitionKey { get; init; }
    public string? SfListenerName { get; init; }
    
    public SetBackendServiceConfig()
    {
        if (BaseUrl == null && BackendId == null || BaseUrl != null && BackendId != null)
        {
            throw new ArgumentException("You need to specify either base-url or backend-id but not both.");
        }
    }
}
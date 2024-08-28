namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public class IpFilterConfig
{
    public required string Action { get; init; }
    public string[]? Addresses { get; init; }
    public AddressRange[]? AddressRanges { get; init; }
}

public class AddressRange
{
    public required string From { get; init; }
    public required string To { get; init; }
}
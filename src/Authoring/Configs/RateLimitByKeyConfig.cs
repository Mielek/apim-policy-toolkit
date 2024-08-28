namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public class RateLimitByKeyConfig
{
    public required int Calls { get; init; }
    public required int RenewalPeriod { get; init; }
    public required string CounterKey { get; init; }

    public bool? IncrementCondition { get; init; }
    public int? IncrementCount { get; init; }
    public string? RetryAfterHeaderName { get; init; }
    public string? RetryAfterVariableName { get; init; }
    public string? RemainingCallsHeaderName { get; init; }
    public string? RemainingCallsVariableName { get; init; }
    public string? TotalCallsHeaderName { get; init; }
}
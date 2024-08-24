namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public class RateLimitConfig
{
    public required int Calls { get; init; }
    public required int RenewalPeriod { get; init; }

    public string? RetryAfterHeaderName { get; init; }
    public string? RetryAfterVariableName { get; init; }
    public string? RemainingCallsHeaderName { get; init; }
    public string? RemainingCallsVariableName { get; init; }
    public string? TotalCallsHeaderName { get; init; }

    public ApiRateLimit[]? Apis { get; init; }
}

public class ApiRateLimit : EntityLimitConfig
{
    public OperationRateLimit[]? Operations { get; init; }
}

public class OperationRateLimit : EntityLimitConfig
{
}

public abstract class EntityLimitConfig
{
    public string? Name { get; init; }
    public string? Id { get; init; }
    public required int Calls { get; init; }
    public required int RenewalPeriod { get; init; }

    public EntityLimitConfig()
    {
        if (Name is null && Id is null)
        {
            throw new ArgumentNullException("Name or Id need to be specified");
        }
    }
}
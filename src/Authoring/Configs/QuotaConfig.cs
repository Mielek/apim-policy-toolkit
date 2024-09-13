namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

/// <summary>
/// TODO
/// </summary>
public record QuotaConfig : BaseQuotaConfig
{
    /// <summary>
    /// TODO
    /// </summary>
    public required int RenewalPeriod { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    public ApiQuota[]? Apis { get; init; }
}

/// <summary>
/// TODO
/// </summary>
public record ApiQuota : EntityQuotaConfig
{
    /// <summary>
    /// TODO
    /// </summary>
    public OperationQuota[]? Operations { get; init; }
}

/// <summary>
/// TODO
/// </summary>
public record OperationQuota : EntityQuotaConfig
{
}

public abstract record EntityQuotaConfig : BaseQuotaConfig
{
    /// <summary>
    /// TODO
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    public string? Id { get; init; }

    public EntityQuotaConfig()
    {
        if (Name is null && Id is null)
        {
            throw new ArgumentNullException($"{nameof(Name)} or {nameof(Id)} need to be specified");
        }
    }
}

public abstract record BaseQuotaConfig
{
    /// <summary>
    /// TODO
    /// </summary>
    public int? Calls { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    public int? Bandwidth { get; init; }

    public BaseQuotaConfig()
    {
        if (this.Calls is null && this.Bandwidth is null)
        {
            throw new ArgumentNullException($"{nameof(Calls)} or {nameof(Bandwidth)} need to be specified");
        }
    }
}
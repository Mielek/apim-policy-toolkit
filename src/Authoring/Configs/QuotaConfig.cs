namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

/// <summary>
/// TODO
/// </summary>
public class QuotaConfig : BaseQuotaConfig
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
public class ApiQuota : EntityQuotaConfig
{
    /// <summary>
    /// TODO
    /// </summary>
    public OperationQuota[]? Operations { get; init; }
}

/// <summary>
/// TODO
/// </summary>
public class OperationQuota : EntityQuotaConfig
{
}

public abstract class EntityQuotaConfig : BaseQuotaConfig
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

public abstract class BaseQuotaConfig
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
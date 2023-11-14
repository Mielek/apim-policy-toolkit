using Mielek.Model.Expressions;

namespace Mielek.Model.Policies;

public sealed record CacheLookupPolicy(
    bool VaryByDeveloper,
    bool VaryByDeveloperGroup,
    CacheLookupCachingType? CatchingType = null,
    CacheLookupDownstreamCachingType? DownstreamCachingType = null,
    bool? MustRevalidate = null,
    IExpression<bool>? AllowPrivateResponseCaching = null,
    ICollection<string>? VaryByHeaders = null,
    ICollection<string>? VaryByQueryParameters = null
) : Visitable<CacheLookupPolicy>, IPolicy;
public enum CacheLookupCachingType { PreferExternal, External, Internal }
public enum CacheLookupDownstreamCachingType { None, Private, Public }

public sealed record CacheStorePolicy(
    IExpression<uint> Duration,
    bool? CacheResponse = null
) : Visitable<CacheStorePolicy>, IPolicy;

public sealed record CacheLookupValuePolicy(
    IExpression<string> Key,
    string VariableName,
    IExpression<string>? DefaultValue,
    CacheLookupValueCachingType? CachingType
) : Visitable<CacheLookupValuePolicy>, IPolicy;
public enum CacheLookupValueCachingType { Internal, External, PreferExternal }

public sealed record CacheStoreValuePolicy(
    IExpression<string> Key,
    IExpression<string> Value,
    IExpression<uint> Duration,
    CacheStoreValuePolicyCachingType? CachingType
) : Visitable<CacheStoreValuePolicy>, IPolicy;
public enum CacheStoreValuePolicyCachingType { Internal, External, PreferExternal }

public sealed record CacheRemoveValuePolicy(
    IExpression<string> Key,
    CacheRemoveValuePolicyCachingType? CachingType
) : Visitable<CacheStoreValuePolicy>, IPolicy;
public enum CacheRemoveValuePolicyCachingType { Internal, External, PreferExternal }

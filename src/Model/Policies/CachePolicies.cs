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


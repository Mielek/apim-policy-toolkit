namespace Mielek.Model.Policies;

public sealed record BasePolicy() : Visitable<BasePolicy>, IPolicy;

public sealed record IncludeFragmentPolicy(string FragmentId) : Visitable<IncludeFragmentPolicy>, IPolicy;

using Mielek.Model.Policies;

namespace Mielek.Model;

public sealed record PolicyDocument(
    ICollection<IPolicy>? Inbound = null,
    ICollection<IPolicy>? Backend = null,
    ICollection<IPolicy>? Outbound = null,
    ICollection<IPolicy>? OnError = null
) : Visitable<PolicyDocument>;

public sealed record PolicyFragment(ICollection<IPolicy> Policies): Visitable<PolicyFragment>;
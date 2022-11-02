using Mielek.Model;
using Mielek.Model.Policies;

namespace Mielek.Builders;
public class PolicyDocumentBuilder
{
    public static PolicyDocumentBuilder Create() => new();

    ICollection<IPolicy>? _inbound;
    ICollection<IPolicy>? _backend;
    ICollection<IPolicy>? _outbound;
    ICollection<IPolicy>? _onError;

    PolicyDocumentBuilder() { }

    public PolicyDocumentBuilder Inbound(Action<PolicySectionBuilder> configurator)
    {
        _inbound = BuildSection(configurator);
        return this;
    }

    public PolicyDocumentBuilder Backend(Action<PolicySectionBuilder> configurator)
    {
        _backend = BuildSection(configurator);
        return this;
    }

    public PolicyDocumentBuilder Outbound(Action<PolicySectionBuilder> configurator)
    {
        _outbound = BuildSection(configurator);
        return this;
    }

    public PolicyDocumentBuilder OnError(Action<PolicySectionBuilder> configurator)
    {
        _onError = BuildSection(configurator);
        return this;
    }

    private ICollection<IPolicy> BuildSection(Action<PolicySectionBuilder> configurator)
    {
        var builder = new PolicySectionBuilder();
        configurator(builder);
        return builder.Build();
    }

    public PolicyDocument Build()
    {
        return new PolicyDocument(_inbound, _backend, _outbound, _onError);
    }
}
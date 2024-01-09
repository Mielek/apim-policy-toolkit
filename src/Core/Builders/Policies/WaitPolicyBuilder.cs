namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(BackendSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class WaitPolicyBuilder<TSectionBuilder> where TSectionBuilder : PolicySectionBuilder, new()
{
    public enum WaitFor { All, Any }

    private WaitFor? _for;
    [IgnoreBuilderField]
    private ICollection<XElement>? _policies;

    public WaitPolicyBuilder<TSectionBuilder> Policies(Action<TSectionBuilder> configurator)
    {
        var builder = new TSectionBuilder();
        configurator(builder);
        _policies = builder.Build();
        return this;
    }

    public XElement Build()
    {
        if (_policies == null) throw new NullReferenceException();

        var children = ImmutableArray.CreateBuilder<object>();
        if (_for != null)
        {
            children.Add(new XAttribute("for", TranslateFor(_for)));
        }

        children.AddRange(_policies.ToArray());

        return new XElement("wait", _for);
    }
    private static string TranslateFor(WaitFor? waitFor) => waitFor switch
    {
        WaitFor.All => "all",
        WaitFor.Any => "any",
        _ => throw new Exception(),
    };
}
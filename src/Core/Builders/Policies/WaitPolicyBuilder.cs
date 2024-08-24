using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(OutboundSectionBuilder)),
    AddToSectionBuilder(typeof(BackendSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class WaitPolicyBuilder<TSectionBuilder> : BaseBuilder<WaitPolicyBuilder<TSectionBuilder>>
    where TSectionBuilder : PolicySectionBuilder, new()
{
    public enum WaitFor { All, Any }

    private WaitFor? _for;

    [IgnoreBuilderField] private ICollection<XElement>? _policies;

    public WaitPolicyBuilder<TSectionBuilder> Policies(Action<TSectionBuilder> configurator)
    {
        var builder = new TSectionBuilder();
        configurator(builder);
        _policies = builder.Build();
        return this;
    }

    public XElement Build()
    {
        if (_policies == null) throw new PolicyValidationException("Policies are required for Wait");

        var element = CreateElement("wait");
        if (_for != null)
        {
            element.Add(new XAttribute("for", TranslateFor(_for)));
        }

        element.Add(_policies.ToArray());

        return element;
    }

    private static string TranslateFor(WaitFor? waitFor) => waitFor switch
    {
        WaitFor.All => "all",
        WaitFor.Any => "any",
        _ => throw new PolicyValidationException("Invalid value for WaitFor"),
    };
}
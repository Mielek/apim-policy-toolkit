using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class QuotaPolicyBuilder : BaseBuilder<QuotaPolicyBuilder>
{
    private uint? _renewalPeriod;
    private uint? _calls;
    private uint? _bandwidth;

    [IgnoreBuilderField]
    private ImmutableList<XElement>.Builder? _apis;

    public QuotaPolicyBuilder Api(Action<QuotaApiBuilder> configurator)
    {
        var builder = new QuotaApiBuilder();
        configurator(builder);
        (_apis ??= ImmutableList.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }

    public XElement Build()
    {
        if (!_renewalPeriod.HasValue) throw new PolicyValidationException("RenewalPeriod is required for Quota");

        var element = this.CreateElement("quota");
        element.Add(new XAttribute("renewal-period", _renewalPeriod));

        if (_calls != null)
        {
            element.Add(new XAttribute("calls", _calls));
        }
        if (_bandwidth != null)
        {
            element.Add(new XAttribute("bandwidth", _bandwidth));
        }

        if (_apis != null && _apis.Count > 0)
        {
            element.Add(_apis.ToArray());
        }

        return element;
    }
}

[GenerateBuilderSetters]
public partial class QuotaApiBuilder
{
    private uint? _calls;
    private string? _name;
    private string? _id;

    [IgnoreBuilderField]
    private ImmutableList<XElement>.Builder? _operations;

    public QuotaApiBuilder Api(Action<QuotaOperationBuilder> configurator)
    {
        var builder = new QuotaOperationBuilder();
        configurator(builder);
        (_operations ??= ImmutableList.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }

    public XElement Build()
    {
        if (!_calls.HasValue) throw new NullReferenceException();
        if (_name == null && _id == null) throw new NullReferenceException();

        var children = ImmutableArray.CreateBuilder<object>();
        children.Add(new XAttribute("calls", _calls));

        if (_name != null)
        {
            children.Add(new XAttribute("name", _name));
        }
        if (_id != null)
        {
            children.Add(new XAttribute("id", _id));
        }

        if (_operations != null && _operations.Count > 0)
        {
            children.AddRange(_operations.ToArray());
        }

        return new XElement("api", children.ToArray());
    }
}

[GenerateBuilderSetters]
public partial class QuotaOperationBuilder
{
    private uint? _calls;
    private string? _name;
    private string? _id;

    public XElement Build()
    {
        if (!_calls.HasValue) throw new NullReferenceException();
        if (_name == null && _id == null) throw new NullReferenceException();

        var children = ImmutableArray.CreateBuilder<object>();
        children.Add(new XAttribute("calls", _calls));

        if (_name != null)
        {
            children.Add(new XAttribute("name", _name));
        }
        if (_id != null)
        {
            children.Add(new XAttribute("id", _id));
        }

        return new XElement("operation", children.ToArray());
    }
}
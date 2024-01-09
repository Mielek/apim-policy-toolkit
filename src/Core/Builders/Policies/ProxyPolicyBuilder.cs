namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class ProxyPolicyBuilder
{
    private string? _url;
    private string? _username;
    private string? _password;

    public XElement Build()
    {
        if (_url == null) throw new NullReferenceException();
        var children = ImmutableArray.CreateBuilder<object>();

        children.Add(new XAttribute("url", _url));

        if (_username != null)
        {
            children.Add(new XAttribute("username", _username));
        }

        if (_password != null)
        {
            children.Add(new XAttribute("password", _password));
        }

        return new XElement("proxy", children.ToArray());
    }
}
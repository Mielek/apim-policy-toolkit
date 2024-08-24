using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class ProxyPolicyBuilder : BaseBuilder<ProxyPolicyBuilder>
{
    private string? _url;
    private string? _username;
    private string? _password;

    public XElement Build()
    {
        if (_url == null) throw new PolicyValidationException("Url is required for Proxy");

        var element = CreateElement("proxy");

        element.Add(new XAttribute("url", _url));

        if (_username != null)
        {
            element.Add(new XAttribute("username", _username));
        }

        if (_password != null)
        {
            element.Add(new XAttribute("password", _password));
        }

        return element;
    }
}
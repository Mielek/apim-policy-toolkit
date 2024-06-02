using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class AuthenticationBasicPolicyBuilder : BaseBuilder<AuthenticationBasicPolicyBuilder>
{
    private string? _username;
    private string? _password;

    public XElement Build()
    {
        if (_username == null) throw new PolicyValidationException("Username is required for AuthenticationBasic");
        if (_password == null) throw new PolicyValidationException("Password is required for AuthenticationBasic");

        var element = this.CreateElement("authentication-basic");
        element.Add(new XAttribute("username", _username));
        element.Add(new XAttribute("password", _password));
        return element;
    }
}
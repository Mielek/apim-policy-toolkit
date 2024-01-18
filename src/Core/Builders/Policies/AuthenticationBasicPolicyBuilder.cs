using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class AuthenticationBasicPolicyBuilder
{
    private string? _username;
    private string? _password;

    public XElement Build()
    {
        if (_username == null) throw new PolicyValidationException("Username is required for AuthenticationBasic");
        if (_password == null) throw new PolicyValidationException("Password is required for AuthenticationBasic");

        return new XElement("authentication-basic", new object[] {
                new XAttribute("username", _username),
                new XAttribute("password", _password),
            });
    }
}
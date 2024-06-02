using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class AuthenticationCertificatePolicyBuilder : BaseBuilder<AuthenticationBasicPolicyBuilder>
{
    private string? _thumbprint;
    private string? _certificateId;
    private IExpression<string>? _body;
    private string? _password;

    public XElement Build()
    {
        if ((_thumbprint == null) == (_certificateId == null))
            throw new PolicyValidationException(
                "Either thumbprint or certificate-id is required for AuthenticationCertificate");

        if (_password != null && _body == null)
            throw new PolicyValidationException("Password is only valid with body for AuthenticationCertificate");

        var element = this.CreateElement("authentication-certificate");

        if (_thumbprint != null)
        {
            element.Add(new XAttribute("thumbprint", _thumbprint));
        }

        if (_certificateId != null)
        {
            element.Add(new XAttribute("certificate-id", _certificateId));
        }

        if (_body != null)
        {
            element.Add(_body.GetXAttribute("body"));
        }

        if (_password != null)
        {
            element.Add(new XAttribute("password", _password));
        }

        return element;
    }
}
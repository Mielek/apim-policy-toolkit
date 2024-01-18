using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[GenerateBuilderSetters]
[ 
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class AuthenticationCertificatePolicyBuilder
{
    private string? _thumbprint;
    private string? _certificateId;
    private IExpression<string>? _body;
    private string? _password;

    public XElement Build()
    {
        if ((_thumbprint == null) == (_certificateId == null)) throw new PolicyValidationException("Either thumbprint or certificate-id is required for AuthenticationCertificate");
        if (_password != null && _body == null) throw new PolicyValidationException("Password is only valid with body for AuthenticationCertificate");


        var attributes = ImmutableArray.CreateBuilder<object>();

        if (_thumbprint != null)
        {
            attributes.Add(new XAttribute("thumbprint", _thumbprint));
        }

        if (_certificateId != null)
        {
            attributes.Add(new XAttribute("certificate-id", _certificateId));
        }

        if (_body != null)
        {
            attributes.Add(_body.GetXAttribute("body"));
        }

        if (_password != null)
        {
            attributes.Add(new XAttribute("password", _password));
        }


        return new XElement("authentication-certificate", attributes.ToArray());
    }
}

using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class
    AuthenticationManagedIdentityPolicyBuilder : BaseBuilder<AuthenticationManagedIdentityPolicyBuilder>
{
    private string? _resource;
    private string? _clientId;
    private string? _outputTokenVariableName;
    private bool? _ignoreError;

    public XElement Build()
    {
        if (_resource == null)
            throw new PolicyValidationException("Resource is required for AuthenticationManagedIdentity");

        var element = this.CreateElement("authentication-managed-identity");

        element.Add(new XAttribute("resource", _resource));

        if (_clientId != null)
        {
            element.Add(new XAttribute("client-id", _clientId));
        }

        if (_outputTokenVariableName != null)
        {
            element.Add(new XAttribute("output-token-variable-name", _outputTokenVariableName));
        }

        if (_ignoreError != null)
        {
            element.Add(new XAttribute("ignore-error", _ignoreError));
        }

        return element;
    }
}
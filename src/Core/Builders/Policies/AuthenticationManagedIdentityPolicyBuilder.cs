namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class AuthenticationManagedIdentityPolicyBuilder
{
    private string? _resource;
    private string? _clientId;
    private string? _outputTokenVariableName;
    private bool? _ignoreError;

    public XElement Build()
    {
        if (_resource == null) throw new NullReferenceException();

        var attributes = ImmutableArray.CreateBuilder<object>();

        attributes.Add(new XAttribute("resource", _resource));
        if (_clientId != null)
        {
            attributes.Add(new XAttribute("client-id", _clientId));
        }
        if (_outputTokenVariableName != null)
        {
            attributes.Add(new XAttribute("output-token-variable-name", _outputTokenVariableName));
        }
        if (_ignoreError != null)
        {
            attributes.Add(new XAttribute("ignore-error", _ignoreError));
        }

        return new XElement("authentication-managed-identity", attributes.ToArray());
    }
}
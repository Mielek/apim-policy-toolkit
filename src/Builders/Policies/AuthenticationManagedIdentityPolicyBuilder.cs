namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Generators.Attributes;

    [GenerateBuilderSetters]
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
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder AuthenticationManagedIdentity(Action<AuthenticationManagedIdentityPolicyBuilder> configurator)
        {
            var builder = new AuthenticationManagedIdentityPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}
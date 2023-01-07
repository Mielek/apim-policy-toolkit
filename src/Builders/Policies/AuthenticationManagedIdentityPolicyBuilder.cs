namespace Mielek.Builders.Policies
{
    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class AuthenticationManagedIdentityPolicyBuilder
    {
        string? _resource;
        string? _clientId;
        string? _outputTokenVariableName;
        bool? _ignoreError;

        public AuthenticationManagedIdentityPolicy Build()
        {
            if (_resource == null) throw new NullReferenceException();

            return new AuthenticationManagedIdentityPolicy(_resource, _clientId, _outputTokenVariableName, _ignoreError);
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
namespace Mielek.Builders.Policies
{
    using Mielek.Generator.Attributes;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class AuthenticationBasicPolicyBuilder
    {
        private string? _username;
        private string? _password;

        public AuthenticationBasicPolicy Build()
        {
            if (_username == null) throw new NullReferenceException();
            if (_password == null) throw new NullReferenceException();

            return new AuthenticationBasicPolicy(_username, _password);
        }
    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder AuthenticationBasic(Action<AuthenticationBasicPolicyBuilder> configurator)
        {
            var builder = new AuthenticationBasicPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}
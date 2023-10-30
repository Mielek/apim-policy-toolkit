namespace Mielek.Builders.Policies
{
    using Mielek.Generator.Attributes;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class ProxyPolicyBuilder
    {
        private string? _url;
        private string? _username;
        private string? _password;

        public ProxyPolicy Build()
        {
            if (_url == null) throw new NullReferenceException();

            return new ProxyPolicy(_url, _username, _password);
        }
    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder AuthenticationCertificate(Action<ProxyPolicyBuilder> configurator)
        {
            var builder = new ProxyPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}
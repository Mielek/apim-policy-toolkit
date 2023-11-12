namespace Mielek.Builders.Policies
{
    using Mielek.Generators.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class AuthenticationCertificatePolicyBuilder
    {
        private string? _thumbprint;
        private string? _certificateId;
        private IExpression<string>? _body;
        private string? _password;

        public AuthenticationCertificatePolicy Build()
        {
            if ((_thumbprint == null) == (_certificateId == null)) throw new Exception();
            if (_password != null && _body == null) throw new Exception();

            return new AuthenticationCertificatePolicy(_thumbprint, _certificateId, _body, _password);
        }
    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder AuthenticationCertificate(Action<AuthenticationCertificatePolicyBuilder> configurator)
        {
            var builder = new AuthenticationCertificatePolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}
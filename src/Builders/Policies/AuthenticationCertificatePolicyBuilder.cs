namespace Mielek.Builders.Policies
{
    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class AuthenticationCertificatePolicyBuilder
    {
        string? _thumbprint;
        string? _certificateId;
        IExpression<string>? _body;
        string? _password;

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
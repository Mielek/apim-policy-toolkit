namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Builders.Expressions;
    using Mielek.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class AuthenticationCertificatePolicyBuilder
    {
        private string? _thumbprint;
        private string? _certificateId;
        private IExpression<string>? _body;
        private string? _password;

        public XElement Build()
        {
            if ((_thumbprint == null) == (_certificateId == null)) throw new Exception();
            if (_password != null && _body == null) throw new Exception();


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
                attributes.Add(new XAttribute("body", _body.GetXText()));
            }

            if (_password != null)
            {
                attributes.Add(new XAttribute("password", _password));
            }


            return new XElement("authentication-certificate", attributes.ToArray());
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
namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class ProxyPolicyBuilder
    {
        private string? _url;
        private string? _username;
        private string? _password;

        public XElement Build()
        {
            if (_url == null) throw new NullReferenceException();
            var children = ImmutableArray.CreateBuilder<object>();

            children.Add(new XAttribute("url", _url));

            if (_username != null)
            {
                children.Add(new XAttribute("username", _username));
            }

            if (_password != null)
            {
                children.Add(new XAttribute("password", _password));
            }

            return new XElement("proxy", children.ToArray());
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
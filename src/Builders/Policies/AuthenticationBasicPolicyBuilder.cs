namespace Mielek.Builders.Policies
{
    using System.Xml.Linq;

    using Mielek.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class AuthenticationBasicPolicyBuilder
    {
        private string? _username;
        private string? _password;

        public XElement Build()
        {
            if (_username == null) throw new NullReferenceException();
            if (_password == null) throw new NullReferenceException();

            return new XElement("authentication-basic", new object[] {
                new XAttribute("username", _username),
                new XAttribute("password", _password),
            });
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
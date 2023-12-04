namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
    using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class SendOneWayRequestPolicyBuilder
    {
        public enum SendOneWayRequestMode { New, Copy }

        private SendOneWayRequestMode? _mode;
        private uint? _timeout;
        private IExpression<string>? _setUrl;
        [IgnoreBuilderField]
        private XElement? _setMethod;
        private IExpression<string>? _setBody;

        [IgnoreBuilderField]
        private ImmutableList<XElement>.Builder? _setHeaders;

        [IgnoreBuilderField]
        private XElement? _authenticationCertificate;

        public SendOneWayRequestPolicyBuilder SetMethod(Action<SetMethodPolicyBuilder> configurator)
        {
            var builder = new SetMethodPolicyBuilder();
            configurator(builder);
            _setMethod = builder.Build();
            return this;
        }

        public SendOneWayRequestPolicyBuilder SetHeader(Action<SetHeaderPolicyBuilder> configurator)
        {
            var builder = new SetHeaderPolicyBuilder();
            configurator(builder);
            (_setHeaders ??= ImmutableList.CreateBuilder<XElement>()).Add(builder.Build());
            return this;
        }

        public SendOneWayRequestPolicyBuilder AuthenticationCertificate(Action<AuthenticationCertificatePolicyBuilder> configurator)
        {
            var builder = new AuthenticationCertificatePolicyBuilder();
            configurator(builder);
            _authenticationCertificate = builder.Build();
            return this;
        }

        public XElement Build()
        {
            if (_mode != SendOneWayRequestMode.Copy)
            {
                if (_setUrl == null) throw new NullReferenceException();
                if (_setMethod == null) throw new NullReferenceException();
            }

            var children = ImmutableArray.CreateBuilder<object>();

            if (_mode != null)
            {
                children.Add(new XAttribute("mode", TranslateMode(_mode)));
            }
            if (_timeout != null)
            {
                children.Add(new XAttribute("timeout", _timeout));
            }
            if (_setMethod != null)
            {
                children.Add(_setMethod);
            }
            if (_setUrl != null)
            {
                children.Add(new XElement("set-url", _setUrl.GetXText()));
            }
            if (_setHeaders != null && _setHeaders.Count > 0)
            {
                children.AddRange(_setHeaders.ToArray());
            }
            if (_setBody != null)
            {
                children.Add(new XElement("set-body", _setBody.GetXText()));
            }
            if (_authenticationCertificate != null)
            {
                children.Add(_authenticationCertificate);
            }

            return new XElement("send-one-way-request", children.ToArray());
        }

        private static string TranslateMode(SendOneWayRequestMode? mode) => mode switch
        {
            SendOneWayRequestMode.Copy => "copy",
            SendOneWayRequestMode.New => "new",
            _ => throw new Exception(),
        };
    }
}


namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders
{
    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder SendOneWayRequest(Action<SendOneWayRequestPolicyBuilder> configurator)
        {
            var builder = new SendOneWayRequestPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}

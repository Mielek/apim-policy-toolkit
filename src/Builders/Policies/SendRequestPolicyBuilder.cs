namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Builders.Expressions;
    using Mielek.Generators.Attributes;


    [GenerateBuilderSetters]
    public partial class SendRequestPolicyBuilder
    {
        private string? _responseVariableName;
        private SendRequestMode? _mode;
        private uint? _timeout;
        private bool? _ignoreError;
        private IExpression<string>? _setUrl;
        [IgnoreBuilderField]
        private XElement? _setMethod;
        private IExpression<string>? _setBody;

        [IgnoreBuilderField]
        private ImmutableList<XElement>.Builder? _setHeaders;

        [IgnoreBuilderField]
        private XElement? _authenticationCertificate;

        public SendRequestPolicyBuilder SetMethod(Action<SetMethodPolicyBuilder> configurator)
        {
            var builder = new SetMethodPolicyBuilder();
            configurator(builder);
            _setMethod = builder.Build();
            return this;
        }

        public SendRequestPolicyBuilder SetHeader(Action<SetHeaderPolicyBuilder> configurator)
        {
            var builder = new SetHeaderPolicyBuilder();
            configurator(builder);
            (_setHeaders ??= ImmutableList.CreateBuilder<XElement>()).Add(builder.Build());
            return this;
        }

        public SendRequestPolicyBuilder AuthenticationCertificate(Action<AuthenticationCertificatePolicyBuilder> configurator)
        {
            var builder = new AuthenticationCertificatePolicyBuilder();
            configurator(builder);
            _authenticationCertificate = builder.Build();
            return this;
        }

        public XElement Build()
        {
            if (_responseVariableName == null) throw new NullReferenceException();
            if (_mode != SendRequestMode.Copy)
            {
                if (_setUrl == null) throw new NullReferenceException();
                if (_setMethod == null) throw new NullReferenceException();
            }

            var children = ImmutableArray.CreateBuilder<object>();

            if (_mode != null)
            {
                children.Add(new XAttribute("mode", TranslateMode(_mode)));
            }
            children.Add(new XAttribute("response-variable-name", _responseVariableName));
            if (_timeout != null)
            {
                children.Add(new XAttribute("timeout", _timeout));
            }
            if (_ignoreError != null)
            {
                children.Add(new XAttribute("ignore-error", _ignoreError));
            }

            if(_setMethod != null)
            {
                children.Add(_setMethod);
            }
            if(_setUrl != null)
            {
                children.Add(new XElement("set-url", _setUrl.GetXText()));
            }
            if(_setHeaders != null && _setHeaders.Count > 0)
            {
                children.AddRange(_setHeaders.ToArray());
            }
            if(_setBody != null)
            {
                children.Add(new XElement("set-body", _setBody.GetXText()));
            }
            if(_authenticationCertificate != null)
            {
                children.Add(_authenticationCertificate);
            }

            return new XElement("send-request", children.ToArray());
        }

        public enum SendRequestMode { New, Copy }
        private static string TranslateMode(SendRequestMode? mode) => mode switch
        {
            SendRequestMode.Copy => "copy",
            SendRequestMode.New => "new",
            _ => throw new Exception(),
        };


    }
}


namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder SendRequest(Action<SendRequestPolicyBuilder> configurator)
        {
            var builder = new SendRequestPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}

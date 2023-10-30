namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;

    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class SendOneWayRequestPolicyBuilder
    {
        private SendOneWayRequestMode? _mode;
        private uint? _timeout;
        private IExpression<string>? _setUrl;
        [IgnoreBuilderField]
        private SetMethodPolicy? _setMethod;
        private IExpression<string>? _setBody;

        [IgnoreBuilderField]
        private ImmutableList<SetHeaderPolicy>.Builder? _setHeaders;

        [IgnoreBuilderField]
        private AuthenticationCertificatePolicy? _authenticationCertificate;

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
            (_setHeaders ??= ImmutableList.CreateBuilder<SetHeaderPolicy>()).Add(builder.Build());
            return this;
        }

        public SendOneWayRequestPolicyBuilder AuthenticationCertificate(Action<AuthenticationCertificatePolicyBuilder> configurator)
        {
            var builder = new AuthenticationCertificatePolicyBuilder();
            configurator(builder);
            _authenticationCertificate = builder.Build();
            return this;
        }

        public SendOneWayRequestPolicy Build()
        {
            if (_mode != SendOneWayRequestMode.Copy)
            {
                if (_setUrl == null) throw new NullReferenceException();
                if (_setMethod == null) throw new NullReferenceException();
            }
            return new SendOneWayRequestPolicy(_mode, _timeout, _setUrl, _setMethod, _setBody, _setHeaders?.ToImmutable(), _authenticationCertificate);
        }
    }
}


namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

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

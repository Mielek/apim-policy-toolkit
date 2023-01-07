namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;

    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class SendRequestPolicyBuilder
    {
        string? _responseVariableName;
        SendRequestMode? _mode;
        uint? _timeout;
        bool? _ignoreError;
        IExpression<string>? _setUrl;
        [IgnoreBuilderField]
        SetMethodPolicy? _setMethod;
        IExpression<string>? _setBody;

        [IgnoreBuilderField]
        ImmutableList<SetHeaderPolicy>.Builder? _setHeaders;

        [IgnoreBuilderField]
        AuthenticationCertificatePolicy? _authenticationCertificate;

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
            (_setHeaders ??= ImmutableList.CreateBuilder<SetHeaderPolicy>()).Add(builder.Build());
            return this;
        }

        public SendRequestPolicyBuilder AuthenticationCertificate(Action<AuthenticationCertificatePolicyBuilder> configurator)
        {
            var builder = new AuthenticationCertificatePolicyBuilder();
            configurator(builder);
            _authenticationCertificate = builder.Build();
            return this;
        }

        public SendRequestPolicy Build()
        {
            if (_responseVariableName == null) throw new NullReferenceException();
            if (_mode != SendRequestMode.Copy)
            {
                if (_setUrl == null) throw new NullReferenceException();
                if (_setMethod == null) throw new NullReferenceException();
            }
            return new SendRequestPolicy(_responseVariableName, _mode, _timeout, _ignoreError, _setUrl, _setMethod, _setBody, _setHeaders?.ToImmutable(), _authenticationCertificate);
        }
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

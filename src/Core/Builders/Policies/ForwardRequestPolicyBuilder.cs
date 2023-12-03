namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class ForwardRequestPolicyBuilder
    {
        private uint? _timeout;
        private bool? _followRedirects;
        private bool? _bufferRequestBody;
        private bool? _bufferResponse;
        private bool? _failOnErrorStatusCode;

        public XElement Build()
        {
            var children = ImmutableArray.CreateBuilder<object>();

            if (_timeout != null)
            {
                children.Add(new XAttribute("timeout", _timeout));
            }
            if (_followRedirects != null)
            {
                children.Add(new XAttribute("follow-redirects", _followRedirects));
            }
            if (_bufferRequestBody != null)
            {
                children.Add(new XAttribute("buffer-request-body", _bufferRequestBody));
            }
            if (_bufferResponse != null)
            {
                children.Add(new XAttribute("buffer-response", _bufferResponse));
            }
            if (_failOnErrorStatusCode != null)
            {
                children.Add(new XAttribute("fail-on-error-status-code", _failOnErrorStatusCode));
            }

            return new XElement("forward-request", children.ToArray());
        }
    }
}


namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders
{
    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder ForwardRequest(Action<ForwardRequestPolicyBuilder> configurator)
        {
            var builder = new ForwardRequestPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}


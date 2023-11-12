namespace Mielek.Builders.Policies
{
    using Mielek.Generators.Attributes;
    using Mielek.Model.Policies;


    [GenerateBuilderSetters]
    public partial class ForwardRequestPolicyBuilder
    {
        private uint? _timeout;
        private bool? _followRedirects;
        private bool? _bufferRequestBody;
        private bool? _bufferResponse;
        private bool? _failOnErrorStatusCode;
        
        public ForwardRequestPolicy Build()
        {
            return new ForwardRequestPolicy(_timeout, _followRedirects, _bufferRequestBody, _bufferResponse, _failOnErrorStatusCode);
        }
    }
}


namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

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


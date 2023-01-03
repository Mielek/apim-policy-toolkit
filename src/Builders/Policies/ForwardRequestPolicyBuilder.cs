namespace Mielek.Builders.Policies
{
    using Mielek.Generator.Attributes;
    using Mielek.Model.Policies;


    [GenerateBuilderSetters]
    public partial class ForwardRequestPolicyBuilder
    {
        uint? _timeout;
        bool? _followRedirects;
        bool? _bufferRequestBody;
        bool? _bufferResponse;
        bool? _failOnErrorStatusCode;
        
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


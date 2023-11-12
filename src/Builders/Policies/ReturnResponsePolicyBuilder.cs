namespace Mielek.Builders.Policies
{
    using Mielek.Generators.Attributes;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class ReturnResponsePolicyBuilder
    {
        [IgnoreBuilderField]
        private SetHeaderPolicy? _setHeaderPolicy;
        [IgnoreBuilderField]
        private SetBodyPolicy? _setBodyPolicy;
        [IgnoreBuilderField]
        private SetStatusPolicy? _setStatusPolicy;
        private string? _responseVariableName;

        public ReturnResponsePolicyBuilder SetHeader(Action<SetHeaderPolicyBuilder> configuration)
        {
            var builder = new SetHeaderPolicyBuilder();
            configuration(builder);
            _setHeaderPolicy = builder.Build();
            return this;
        }
        public ReturnResponsePolicyBuilder SetBody(Action<SetBodyPolicyBuilder> configuration)
        {
            var builder = new SetBodyPolicyBuilder();
            configuration(builder);
            _setBodyPolicy = builder.Build();
            return this;
        }
        public ReturnResponsePolicyBuilder SetStatus(Action<SetStatusPolicyBuilder> configuration)
        {
            var builder = new SetStatusPolicyBuilder();
            configuration(builder);
            _setStatusPolicy = builder.Build();
            return this;
        }

        public ReturnResponsePolicy Build()
        {

            return new ReturnResponsePolicy(_setHeaderPolicy, _setBodyPolicy, _setStatusPolicy, _responseVariableName);
        }
    }
}


namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder ReturnResponse(Action<ReturnResponsePolicyBuilder> configurator)
        {
            var builder = new ReturnResponsePolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}

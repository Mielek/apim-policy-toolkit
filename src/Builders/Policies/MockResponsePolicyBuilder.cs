namespace Mielek.Builders.Policies
{
    using Mielek.Generators.Attributes;
    using Mielek.Model.Policies;


    [GenerateBuilderSetters]
    public partial class MockResponsePolicyBuilder
    {
        private uint? _statusCode;
        private string? _contentType;
        
        public MockResponsePolicy Build()
        {
            return new MockResponsePolicy(_statusCode, _contentType);
        }
    }
}


namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder MockResponse(Action<MockResponsePolicyBuilder> configurator)
        {
            var builder = new MockResponsePolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}

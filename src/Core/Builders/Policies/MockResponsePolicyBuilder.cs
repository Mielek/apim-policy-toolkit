namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class MockResponsePolicyBuilder
    {
        private uint? _statusCode;
        private string? _contentType;

        public XElement Build()
        {
            var children = ImmutableArray.CreateBuilder<object>();

            if (_statusCode != null)
            {
                children.Add(new XAttribute("status-code", _statusCode));
            }

            if (_contentType != null)
            {
                children.Add(new XAttribute("content-type", _contentType));
            }

            return new XElement("mock-response", children.ToArray());
        }
    }
}


namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders
{
    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

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

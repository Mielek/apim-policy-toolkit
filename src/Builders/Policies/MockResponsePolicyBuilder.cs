namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Generators.Attributes;

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

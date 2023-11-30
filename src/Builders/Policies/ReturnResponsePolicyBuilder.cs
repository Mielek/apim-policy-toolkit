namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class ReturnResponsePolicyBuilder
    {
        [IgnoreBuilderField]
        private ImmutableList<XElement>.Builder? _setHeaderPolicies;
        [IgnoreBuilderField]
        private XElement? _setBodyPolicy;
        [IgnoreBuilderField]
        private XElement? _setStatusPolicy;
        private string? _responseVariableName;

        public ReturnResponsePolicyBuilder SetHeader(Action<SetHeaderPolicyBuilder> configuration)
        {
            var builder = new SetHeaderPolicyBuilder();
            configuration(builder);
            (_setHeaderPolicies ??= ImmutableList.CreateBuilder<XElement>()).Add(builder.Build());
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

        public XElement Build()
        {
            var children = ImmutableArray.CreateBuilder<object>();
            if (_responseVariableName != null)
            {
                children.Add(new XAttribute("response-variable-name", _responseVariableName));
            }
            if (_setStatusPolicy != null)
            {
                children.Add(_setStatusPolicy);
            }
            if (_setHeaderPolicies != null && _setHeaderPolicies.Count > 0)
            {
                children.AddRange(_setHeaderPolicies.ToImmutable());
            }
            if (_setBodyPolicy != null)
            {
                children.Add(_setBodyPolicy);
            }

            return new XElement("return-response", children.ToArray());
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

namespace Mielek.Builders.Policies
{
    using System.Xml.Linq;

    using Mielek.Builders.Expressions;
    using Mielek.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class SetVariablePolicyBuilder
    {
        private string? _name;
        private IExpression<string>? _value;

        public XElement Build()
        {
            if (_name == null) throw new NullReferenceException();
            if (_value == null) throw new NullReferenceException();

            var children = new[] {
                new XAttribute("name", _name),
                _value.GetXAttribute("value")
            };
            return new XElement("set-variable", children);
        }

    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;
    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder SetVariable(Action<SetVariablePolicyBuilder> configurator)
        {
            var builder = new SetVariablePolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }

    }
}
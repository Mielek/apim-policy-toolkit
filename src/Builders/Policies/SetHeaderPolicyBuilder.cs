namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Builders.Expressions;
    using Mielek.Generators.Attributes;


    [GenerateBuilderSetters]
    public partial class SetHeaderPolicyBuilder
    {
        public enum SetHeaderPolicyExistsAction { Override, Skip, Append, Delete }

        private IExpression<string>? _name;
        private ImmutableList<IExpression<string>>.Builder? _values;
        private IExpression<string>? _existsAction;

        public SetHeaderPolicyBuilder ExistsAction(SetHeaderPolicyExistsAction existsAction)
        {
            return ExistsAction(Translate(existsAction));
        }

        private string Translate(SetHeaderPolicyExistsAction existsAction) => existsAction switch
        {
            SetHeaderPolicyExistsAction.Override => "override",
            SetHeaderPolicyExistsAction.Append => "append",
            SetHeaderPolicyExistsAction.Delete => "delete",
            SetHeaderPolicyExistsAction.Skip => "skip",
            _ => throw new Exception(),
        };

        public XElement Build()
        {
            if (_name == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<object>();
            children.Add(new XAttribute("name", _name.GetXText()));
            if (_existsAction != null)
            {
                children.Add(new XAttribute("exists-action", _existsAction.GetXText()));
            }
            if (_values != null && _values.Count > 0)
            {
                children.AddRange(_values.ToImmutable().Select(v => new XElement("value", v.GetXText())));
            }

            return new XElement("set-header", children.ToArray());
        }

    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder SetHeader(Action<SetHeaderPolicyBuilder> configurator)
        {
            var builder = new SetHeaderPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}
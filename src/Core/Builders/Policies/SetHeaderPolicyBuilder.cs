namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
    using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;


    [GenerateBuilderSetters]
    public partial class SetHeaderPolicyBuilder
    {
        public enum ExistsActionType { Override, Skip, Append, Delete }

        private IExpression<string>? _name;
        private ImmutableList<IExpression<string>>.Builder? _values;
        private IExpression<string>? _existsAction;

        public SetHeaderPolicyBuilder ExistsAction(ExistsActionType existsAction)
        {
            return ExistsAction(Translate(existsAction));
        }

        private string Translate(ExistsActionType existsAction) => existsAction switch
        {
            ExistsActionType.Override => "override",
            ExistsActionType.Append => "append",
            ExistsActionType.Delete => "delete",
            ExistsActionType.Skip => "skip",
            _ => throw new Exception(),
        };

        public XElement Build()
        {
            if (_name == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<object>();
            children.Add(_name.GetXAttribute("name"));
            if (_existsAction != null)
            {
                children.Add(_existsAction.GetXAttribute("exists-action"));
            }
            if (_values != null && _values.Count > 0)
            {
                children.AddRange(_values.ToImmutable().Select(v => new XElement("value", v.GetXText())));
            }

            return new XElement("set-header", children.ToArray());
        }

    }
}

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders
{
    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

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
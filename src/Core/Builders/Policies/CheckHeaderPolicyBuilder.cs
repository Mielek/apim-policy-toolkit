namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
    using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class CheckHeaderPolicyBuilder
    {
        private IExpression<string>? _name;
        private IExpression<string>? _failedCheckHttpCode;
        private IExpression<string>? _failedCheckErrorMessage;
        private IExpression<bool>? _ignoreCase;
        private ImmutableList<IExpression<string>>.Builder? _values;

        public CheckHeaderPolicyBuilder FailedCheckHttpCode(ushort code)
        {
            return FailedCheckHttpCode($"{code}");
        }

        public XElement Build()
        {
            if (_name == null) throw new NullReferenceException();
            if (_failedCheckHttpCode == null) throw new NullReferenceException();
            if (_failedCheckErrorMessage == null) throw new NullReferenceException();
            if (_ignoreCase == null) throw new NullReferenceException();
            if (_values == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<object>();

            children.Add(_name.GetXAttribute("name"));
            children.Add(_failedCheckHttpCode.GetXAttribute("failed-check-httpcode"));
            children.Add(_failedCheckErrorMessage.GetXAttribute("failed-check-error-message"));
            children.Add(_ignoreCase.GetXAttribute("ignore-case"));

            foreach (var value in _values.ToArray())
            {
                children.Add(new XElement("value", value.GetXText()));
            }

            return new XElement("check-header", children.ToArray());
        }
    }
}

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders
{
    using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder CheckHeader(Action<CheckHeaderPolicyBuilder> configurator)
        {
            var builder = new CheckHeaderPolicyBuilder();
            configurator(builder);
            this._sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}
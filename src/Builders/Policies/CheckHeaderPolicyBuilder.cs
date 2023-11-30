namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;
    using System.Xml.Linq;

    using Mielek.Builders.Expressions;
    using Mielek.Generators.Attributes;

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

            children.Add(new XAttribute("name", _name.GetXText()));
            children.Add(new XAttribute("failed-check-httpcode", _failedCheckHttpCode.GetXText()));
            children.Add(new XAttribute("failed-check-error-message", _failedCheckErrorMessage.GetXText()));
            children.Add(new XAttribute("ignore-case", _ignoreCase.GetXText()));

            foreach (var value in _values.ToArray())
            {
                children.Add(new XElement("value", _name.GetXText()));
            }

            return new XElement("check-header", children.ToArray());
        }
    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

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
using System.Collections.Immutable;
using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies
{
    [GenerateBuilderSetters]
    [
        AddToSectionBuilder(typeof(InboundSectionBuilder)),
        AddToSectionBuilder(typeof(PolicyFragmentBuilder))
    ]
    public partial class CheckHeaderPolicyBuilder : BaseBuilder<CheckHeaderPolicyBuilder>
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
            if (_name == null) throw new PolicyValidationException("CheckHeader requires name");
            if (_failedCheckHttpCode == null) throw new PolicyValidationException("CheckHeader requires failed-check-httpcode");
            if (_failedCheckErrorMessage == null) throw new NullReferenceException("CheckHeader requires failed-check-error-message");
            if (_ignoreCase == null) throw new NullReferenceException("CheckHeader requires ignore-case");
            if (_values == null) throw new NullReferenceException("CheckHeader requires values");

            var element = this.CreateElement("check-header");

            element.Add(_name.GetXAttribute("name"));
            element.Add(_failedCheckHttpCode.GetXAttribute("failed-check-httpcode"));
            element.Add(_failedCheckErrorMessage.GetXAttribute("failed-check-error-message"));
            element.Add(_ignoreCase.GetXAttribute("ignore-case"));

            foreach (var value in _values.ToArray())
            {
                element.Add(new XElement("value", value.GetXText()));
            }

            return element;
        }
    }
}
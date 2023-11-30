namespace Mielek.Builders.Policies
{
    using System.Xml.Linq;

    using Mielek.Builders.Expressions;
    using Mielek.Generators.Attributes;

    [GenerateBuilderSetters]
    public partial class SetMethodPolicyBuilder
    {
        private IExpression<string>? _method;

        public SetMethodPolicyBuilder Get()
        {
            return Method(HttpMethod.Get);
        }
        public SetMethodPolicyBuilder Post()
        {
            return Method(HttpMethod.Post);
        }
        public SetMethodPolicyBuilder Head()
        {
            return Method(HttpMethod.Head);
        }
        public SetMethodPolicyBuilder Delete()
        {
            return Method(HttpMethod.Delete);
        }
        public SetMethodPolicyBuilder Put()
        {
            return Method(HttpMethod.Put);
        }
        public SetMethodPolicyBuilder Options()
        {
            return Method(HttpMethod.Options);
        }

        public SetMethodPolicyBuilder Method(HttpMethod method)
        {
            return Method(configurator => configurator.Constant(method.Method));
        }

        public XElement Build()
        {
            if (_method == null) throw new NullReferenceException();

            return new XElement("set-method", _method.GetXText());
        }
    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;
    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder SetMethod(Action<SetMethodPolicyBuilder> configurator)
        {
            var builder = new SetMethodPolicyBuilder();
            configurator(builder);
            _sectionPolicies.Add(builder.Build());
            return this;
        }

    }
}
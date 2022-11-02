namespace Mielek.Builders.Policies
{
    using Mielek.Builders.Expressions;

    using Mielek.Model.Policies;

    public class SetMethodPolicyBuilder
    {
        SetMethodPolicy? _policy;

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

        public SetMethodPolicyBuilder Method(string method)
        {
            return Method(configurator => configurator.Constant(method));
        }

        public SetMethodPolicyBuilder Method(Action<ExpressionBuilder> configurator)
        {
            _policy = new SetMethodPolicy(ExpressionBuilder.BuildFromConfiguration(configurator));
            return this;
        }

        public SetMethodPolicy Build()
        {
            return _policy ?? throw new NullReferenceException();
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
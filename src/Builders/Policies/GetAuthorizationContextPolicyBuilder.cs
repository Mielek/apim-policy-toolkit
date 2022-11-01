using Mielek.Builders.Expressions;
using Mielek.Model.Expressions;
using Mielek.Model.Policies;

namespace Mielek.Builders.Policies
{
    public class GetAuthorizationContextPolicyBuilder
    {

        IExpression? _providerId;
        IExpression? _authorizationId;
        string? _contextVariableName;
        IdentityType? _identityType;
        IExpression? _identity;
        bool? _ignoreError;


        public GetAuthorizationContextPolicyBuilder ProviderId(string value) {
            return ProviderId(config => config.Constant(value));
        }
        public GetAuthorizationContextPolicyBuilder ProviderId(Action<ExpressionBuilder> configurator) {
            _providerId = ExpressionBuilder.BuildFromConfiguration(configurator);
            return this;
        }
        
        public GetAuthorizationContextPolicyBuilder AuthorizationId(string value) {
            return AuthorizationId(config => config.Constant(value));
        }
        public GetAuthorizationContextPolicyBuilder AuthorizationId(Action<ExpressionBuilder> configurator) {
            _authorizationId = ExpressionBuilder.BuildFromConfiguration(configurator);
            return this;
        }

        public GetAuthorizationContextPolicyBuilder ContextVariableName(string value) {
            _contextVariableName = value;
             return this;
        }
        
        public GetAuthorizationContextPolicyBuilder IdentityType(IdentityType value) {
            _identityType = value;
             return this;
        }

        public GetAuthorizationContextPolicyBuilder Identity(string value) {
            return Identity(config => config.Constant(value));
        }
        public GetAuthorizationContextPolicyBuilder Identity(Action<ExpressionBuilder> configurator) {
            _identity = ExpressionBuilder.BuildFromConfiguration(configurator);
            return this;
        }

        public GetAuthorizationContextPolicyBuilder IgnoreError(bool value) {
            _ignoreError = value;
             return this;
        }

        public GetAuthorizationContextPolicy Build()
        {
            if (_providerId == null) throw new NullReferenceException();
            if (_authorizationId == null) throw new NullReferenceException();
            if (_contextVariableName == null) throw new NullReferenceException();

            return new GetAuthorizationContextPolicy(_providerId, _authorizationId, _contextVariableName, _identityType, _identity, _ignoreError);
        }
    }
}
namespace Mielek.Builders
{
    using Mielek.Builders.Policies;
    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder GetAuthorizationContextPolicy(Action<GetAuthorizationContextPolicyBuilder> configurator)
        {
            var builder = new GetAuthorizationContextPolicyBuilder();
            configurator(builder);
            sectionPolicies.Add(builder.Build());
            return this;
        }

    }
}
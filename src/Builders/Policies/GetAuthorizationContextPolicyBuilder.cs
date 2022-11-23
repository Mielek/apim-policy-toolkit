using Mielek.Builders.Expressions;
using Mielek.Model.Expressions;
using Mielek.Model.Policies;
using Mielek.Generator.Attributes;

namespace Mielek.Builders.Policies
{
    [GenerateBuilderSetters]
    public partial class GetAuthorizationContextPolicyBuilder
    {
        IExpression? _providerId;
        IExpression? _authorizationId;
        string? _contextVariableName;
        IdentityType? _identityType;
        IExpression? _identity;
        bool? _ignoreError;

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
            _sectionPolicies.Add(builder.Build());
            return this;
        }

    }
}
namespace Mielek.Builders.Policies
{
    using System.Xml.Linq;
    using System.Collections.Immutable;

    using Mielek.Generators.Attributes;
    using Mielek.Builders.Expressions;

    [GenerateBuilderSetters]
    public partial class GetAuthorizationContextPolicyBuilder
    {
        public enum IdentityTypeEnum { Managed, JWT }

        private IExpression<string>? _providerId;
        private IExpression<string>? _authorizationId;
        private string? _contextVariableName;
        private IdentityTypeEnum? _identityType;
        private IExpression<string>? _identity;
        private bool? _ignoreError;

        public XElement Build()
        {
            if (_providerId == null) throw new NullReferenceException();
            if (_authorizationId == null) throw new NullReferenceException();
            if (_contextVariableName == null) throw new NullReferenceException();

            var children = ImmutableArray.CreateBuilder<object>();

            children.Add(_providerId.GetXAttribute("provider-id"));
            children.Add(_authorizationId.GetXAttribute("authorization-id"));
            children.Add(new XAttribute("context-variable", _contextVariableName));

            if (_identityType != null)
            {
                children.Add(new XAttribute("identity-type", TranslateIdentity(_identityType)));
            }
            if (_identity != null)
            {
                children.Add(_identity.GetXAttribute("identity"));
            }
            if (_ignoreError != null)
            {
                children.Add(new XAttribute("ignore-error", _ignoreError));
            }

            return new XElement("get-authorization-context", children.ToArray());
        }

        private string TranslateIdentity(IdentityTypeEnum? identityType) => identityType switch
        {
            IdentityTypeEnum.Managed => "managed",
            IdentityTypeEnum.JWT => "jwt",
            _ => throw new NotImplementedException(),
        };

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
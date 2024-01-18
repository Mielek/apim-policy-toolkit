using System.Xml.Linq;

using Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Policies;

[GenerateBuilderSetters]
[
    AddToSectionBuilder(typeof(InboundSectionBuilder)),
    AddToSectionBuilder(typeof(PolicyFragmentBuilder))
]
public partial class GetAuthorizationContextPolicyBuilder : BaseBuilder<GetAuthorizationContextPolicyBuilder>
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
        if (_providerId == null) throw new PolicyValidationException("ProviderId is required for GetAuthorizationContext");
        if (_authorizationId == null) throw new PolicyValidationException("AuthorizationId is required for GetAuthorizationContext");
        if (_contextVariableName == null) throw new PolicyValidationException("ContextVariableName is required for GetAuthorizationContext");

        var element = this.CreateElement("get-authorization-context");

        element.Add(_providerId.GetXAttribute("provider-id"));
        element.Add(_authorizationId.GetXAttribute("authorization-id"));
        element.Add(new XAttribute("context-variable", _contextVariableName));

        if (_identityType != null)
        {
            element.Add(new XAttribute("identity-type", TranslateIdentity(_identityType)));
        }
        if (_identity != null)
        {
            element.Add(_identity.GetXAttribute("identity"));
        }
        if (_ignoreError != null)
        {
            element.Add(new XAttribute("ignore-error", _ignoreError));
        }

        return element;
    }

    private string TranslateIdentity(IdentityTypeEnum? identityType) => identityType switch
    {
        IdentityTypeEnum.Managed => "managed",
        IdentityTypeEnum.JWT => "jwt",
        _ => throw new PolicyValidationException("Unknown IdentityType for GetAuthorizationContext"),
    };

}
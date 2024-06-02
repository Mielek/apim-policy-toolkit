using System.Collections.Immutable;
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
public partial class ValidateAzureAdTokenPolicyBuilder : BaseBuilder<ValidateAzureAdTokenPolicyBuilder>
{
    private ImmutableList<string>.Builder? _clientApplicationIds;
    private string? _headerName;
    private string? _queryParameterName;
    private string? _tokenValue;
    private string? _tenantId;
    private ushort? _failedValidationHttpCode;
    private string? _failedValidationErrorMessage;
    private string? _outputTokenVariableName;
    private ImmutableList<string>.Builder? _backendApplicationIds;
    private ImmutableList<IExpression<string>>.Builder? _audiences;

    [IgnoreBuilderField]
    private ImmutableList<XElement>.Builder? _requiredClaims;

    public ValidateAzureAdTokenPolicyBuilder RequiredClaim(Action<ValidateAzureAdTokenClaimBuilder> configurator)
    {
        var builder = new ValidateAzureAdTokenClaimBuilder();
        configurator(builder);
        (_requiredClaims ??= ImmutableList.CreateBuilder<XElement>()).Add(builder.Build());
        return this;
    }

    public XElement Build()
    {
        if (_clientApplicationIds == null) throw new PolicyValidationException("ClientApplicationIds is required for ValidateAzureAdToken");

        var element = CreateElement("validate-azure-ad-token");

        if (_tenantId != null)
        {
            element.Add(new XAttribute("tenant-id", _tenantId));
        }
        if (_headerName != null)
        {
            element.Add(new XAttribute("header-name", _headerName));
        }
        if (_queryParameterName != null)
        {
            element.Add(new XAttribute("query-parameter-name", _queryParameterName));
        }
        if (_tokenValue != null)
        {
            element.Add(new XAttribute("token-value", _tokenValue));
        }
        if (_failedValidationHttpCode != null)
        {
            element.Add(new XAttribute("failed-validation-httpcode", _failedValidationHttpCode));
        }
        if (_failedValidationErrorMessage != null)
        {
            element.Add(new XAttribute("failed-validation-error-message", _failedValidationErrorMessage));
        }
        if (_outputTokenVariableName != null)
        {
            element.Add(new XAttribute("output-token-variable-name", _outputTokenVariableName));
        }

        element.Add(new XElement("client-application-ids", _clientApplicationIds.Select(i => new XElement("application-id", i)).ToArray()));

        if (_backendApplicationIds != null && _backendApplicationIds.Count > 0)
        {
            element.Add(new XElement("backend-application-ids", _backendApplicationIds.Select(i => new XElement("application-id", i)).ToArray()));
        }
        if (_audiences != null && _audiences.Count > 0)
        {
            element.Add(new XElement("audiences", _audiences.Select(i => new XElement("audience", i.GetXText())).ToArray()));
        }
        if (_requiredClaims != null && _requiredClaims.Count > 0)
        {
            element.Add(new XElement("required-claims", _requiredClaims.ToArray()));
        }

        return element;
    }
}

[GenerateBuilderSetters]
public partial class ValidateAzureAdTokenClaimBuilder
{
    public enum ClaimMatch { All, Any }

    private string? _name;
    private ImmutableList<string>.Builder? _values;
    private ClaimMatch? _match;
    private string? _separator;

    public XElement Build()
    {
        if (_name == null) throw new NullReferenceException();
        if (_values == null) throw new NullReferenceException();

        var children = ImmutableArray.CreateBuilder<object>();
        children.Add(new XAttribute("name", _name));
        if (_match != null)
        {
            children.Add(new XAttribute("match", TranslateClaimMatch(_match)));
        }
        if (_separator != null)
        {
            children.Add(new XAttribute("separator", _separator));
        }

        children.AddRange(_values.Select(v => new XElement("value", v)));

        return new XElement("claim", children.ToArray());
    }

    private string TranslateClaimMatch(ClaimMatch? match) => match switch
    {
        ClaimMatch.All => "all",
        ClaimMatch.Any => "any",
        _ => throw new PolicyValidationException("Unknown match for ValidateAzureAdTokenClaim"),
    };
}
namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;

    using Mielek.Generator.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class ValidateAzureAdTokenPolicyBuilder
    {
        ImmutableList<string>.Builder? _clientApplicationIds;
        string? _headerName;
        string? _queryParameterName;
        string? _tokenValue;
        string? _tenantId;
        ushort? _failedValidationHttpCode;
        string? _failedValidationErrorMessage;
        string? _outputTokenVariableName;
        ImmutableList<string>.Builder? _backendApplicationIds;
        ImmutableList<IExpression>.Builder? _audiences;
        
        [IgnoreBuilderField]
        ImmutableList<ValidateAzureAdTokenClaim>.Builder? _requiredClaims;

        public ValidateAzureAdTokenPolicyBuilder RequiredClaim(Action<ValidateAzureAdTokenClaimBuilder> configurator)
        {
            var builder = new ValidateAzureAdTokenClaimBuilder();
            configurator(builder);
            (_requiredClaims ??= ImmutableList.CreateBuilder<ValidateAzureAdTokenClaim>()).Add(builder.Build());
            return this;
        }

        public ValidateAzureAdTokenPolicy Build()
        {
            if (_clientApplicationIds == null) throw new NullReferenceException();

            return new ValidateAzureAdTokenPolicy(
                _clientApplicationIds.ToImmutable(),
                _headerName,
                _queryParameterName,
                _tokenValue,
                _tenantId,
                _failedValidationHttpCode,
                _failedValidationErrorMessage,
                _outputTokenVariableName,
                _backendApplicationIds?.ToImmutable(),
                _audiences?.ToImmutable(),
                _requiredClaims?.ToImmutable());
        }
    }

    [GenerateBuilderSetters]
    public partial class ValidateAzureAdTokenClaimBuilder
    {
        string? _name;
        ImmutableList<string>.Builder? _values;
        ValidateAzureAdTokenClaimMatch? _match;
        string? _separator;

        public ValidateAzureAdTokenClaim Build()
        {
            if (_name == null) throw new NullReferenceException();
            if (_values == null) throw new NullReferenceException();

            return new ValidateAzureAdTokenClaim(_name, _values.ToImmutable(), _match, _separator);
        }
    }
}

namespace Mielek.Builders
{
    using Mielek.Builders.Policies;

    public partial class PolicySectionBuilder
    {
        public PolicySectionBuilder ValidateAzureAdToken(Action<ValidateAzureAdTokenPolicyBuilder> configurator)
        {
            var builder = new ValidateAzureAdTokenPolicyBuilder();
            configurator(builder);
            this._sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}
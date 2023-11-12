namespace Mielek.Builders.Policies
{
    using System.Collections.Immutable;

    using Mielek.Generators.Attributes;
    using Mielek.Model.Expressions;
    using Mielek.Model.Policies;

    [GenerateBuilderSetters]
    public partial class ValidateAzureAdTokenPolicyBuilder
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
        private ImmutableList<ValidateAzureAdTokenClaim>.Builder? _requiredClaims;

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
        private string? _name;
        private ImmutableList<string>.Builder? _values;
        private ValidateAzureAdTokenClaimMatch? _match;
        private string? _separator;

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
            _sectionPolicies.Add(builder.Build());
            return this;
        }
    }
}
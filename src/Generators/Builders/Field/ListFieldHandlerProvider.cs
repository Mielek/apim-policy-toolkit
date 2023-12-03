using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Builder.Extensions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Builder.Field;

public class ImmutableListFieldHandlerProvider : IFieldSetterHandlerProvider
{
    private static readonly Regex ListRegex = new(@"^ImmutableList<(.*)>.Builder\??$");
    public bool TryGetHandler(FieldDeclarationSyntax field, [NotNullWhen(true)] out IFieldSetterHandler? handler)
    {
        var type = field.Declaration.Type.ToString();
        var regexMatch = ListRegex.Match(type);
        if (regexMatch.Success)
        {
            var innerType = regexMatch.Groups[1].Value;
            handler = innerType.StartsWith("IExpression")
                ? new ExpressionListFieldHandler(field)
                : new SimpleListFieldHandler(field, innerType);
            return true;
        }

        handler = default;
        return false;
    }

    public class ExpressionListFieldHandler : IFieldSetterHandler
    {
        private readonly FieldDeclarationSyntax _field;

        public ExpressionListFieldHandler(FieldDeclarationSyntax field)
        {
            _field = field;
        }

        public void Handle(BuilderClassBuilder builder)
        {
            var innerType = _field.Declaration.Type.DescendantNodes()
                .OfType<GenericNameSyntax>()
                .Last()
                .DescendantNodes()
                .OfType<TypeSyntax>()
                .Last();

            foreach (var variable in _field.Declaration.Variables)
            {
                var variableName = variable.Identifier.ToString();
                var methodName = variableName.ToListMethodName();
                builder.Method(new BuilderSetMethod(
                    methodName,
                    new[] { $"{innerType} value" },
                    new[] { $"this.{methodName}(config => config.Constant(value));" }
                ));
                builder.Method(new BuilderSetMethod(
                    methodName,
                    new[] { $"Action<ExpressionBuilder<{innerType}>> configurator" },
                    new[] { $"var value = ExpressionBuilder<{innerType}>.BuildFromConfiguration(configurator);",
                            $"({variableName} ??= ImmutableList.CreateBuilder<IExpression<{innerType}>>()).Add(value);" }
                ));
            }
        }
    }

    public class SimpleListFieldHandler : IFieldSetterHandler
    {
        private readonly FieldDeclarationSyntax _field;
        private readonly string _innerType;

        public SimpleListFieldHandler(FieldDeclarationSyntax field, string innerType)
        {
            _field = field;
            _innerType = innerType;
        }

        public void Handle(BuilderClassBuilder builder)
        {
            foreach (var variable in _field.Declaration.Variables)
            {
                var variableName = variable.Identifier.ToString();
                var methodName = variableName.ToListMethodName();
                builder.Method(new BuilderSetMethod(
                    methodName,
                    new[] { $"{_innerType} value" },
                    new[] { $"({variableName} ??= ImmutableList.CreateBuilder<{_innerType}>()).Add(value);" }
                ));
            }
        }
    }
}
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Generator.Builder.Extensions;

namespace Mielek.Generator.Builder.Field;

public class ListFieldHandlerProvider : IFieldSetterHandlerProvider
{
    readonly static Regex ListRegex = new(@"^ImmutableList<(.*)>.Builder\??$");
    public bool TryGetHandler(FieldDeclarationSyntax field, out IFieldSetterHandler handler)
    {
        var type = field.Declaration.Type.ToString();
        var regexMatch = ListRegex.Match(type);
        if (regexMatch.Success)
        {
            var innerType = regexMatch.Groups[1].Value;
            handler = innerType == "IExpression" ? new ExpressionListFieldHandler(field) : new SimpleListFieldHandler(field, innerType);
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
            foreach (var variable in _field.Declaration.Variables)
            {
                var variableName = variable.Identifier.ToString();
                var methodName = variableName.ToListMethodName();
                builder.Method(new BuilderSetMethod(
                    methodName,
                    new[] { "string value" },
                    new[] { $"this.{methodName}(config => config.Constant(value));" }
                ));
                builder.Method(new BuilderSetMethod(
                    methodName,
                    new[] { "Action<ExpressionBuilder> configurator" },
                    new[] { "var value = ExpressionBuilder.BuildFromConfiguration(configurator);",
                            $"({variableName} ??= ImmutableList.CreateBuilder<IExpression>()).Add(value);" }
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
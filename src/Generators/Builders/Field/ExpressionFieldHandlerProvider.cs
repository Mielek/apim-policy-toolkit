
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Generators.Builder.Extensions;

namespace Mielek.Generators.Builder.Field;

public class ExpressionFieldHandlerProvider : IFieldSetterHandlerProvider
{
    public bool TryGetHandler(FieldDeclarationSyntax field, [NotNullWhen(true)] out IFieldSetterHandler? handler)
    {
        var type = field.Declaration.Type.ToString();
        if (type.StartsWith("IExpression"))
        {
            handler = new ExpressionFieldHandler(field);
            return true;
        }

        handler = default;
        return false;
    }

    public class ExpressionFieldHandler : IFieldSetterHandler
    {
        private readonly FieldDeclarationSyntax _field;

        public ExpressionFieldHandler(FieldDeclarationSyntax field)
        {
            _field = field;
        }

        public void Handle(BuilderClassBuilder builder)
        {
            var type = _field.Declaration.Type.DescendantNodes()
                .OfType<TypeSyntax>()
                .Last();

            foreach (var variable in _field.Declaration.Variables)
            {
                var variableName = variable.Identifier.ToString();
                var methodName = variableName.ToMethodName();
                builder.Method(new BuilderSetMethod(
                    methodName,
                    new[] { $"{type} value" },
                    new[] { $"this.{methodName}(config => config.Constant(value));" }
                ));
                builder.Method(new BuilderSetMethod(
                    methodName,
                    new[] { $"Action<ExpressionBuilder<{type}>> configurator" },
                    new[] { $"{variableName} = ExpressionBuilder<{type}>.BuildFromConfiguration(configurator);" }
                ));
            }
        }
    }
}